using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{

    [Header("Debugging")]
    [SerializeField] private bool initilaizeDataIfNull = false;

    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private DataSerializer serializer;

    public static SaveManager Instance;
    private CharacterData curCharData;
    private float saveCooldown = 2f;
    private float lastSaveTime = -Mathf.Infinity;
    private List<ISaver> thingsToSave;

    private string selectedProfileId = "";

    #region Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        this.serializer = new DataSerializer(Application.persistentDataPath, fileName, useEncryption);

    }
    #endregion

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.thingsToSave = FindAllItemsToSave();
        LoadCharacterData(selectedProfileId);
    }

    public void OnSceneUnloaded(Scene scene)
    {
        SaveDataAsync();
    }

    public void RequestSave()
    {

        if (Time.time - lastSaveTime > saveCooldown)
        {
            lastSaveTime = Time.time;
            SaveDataAsync();
        }

    }

    private void SaveDataAsync()
    {

        if (this.curCharData == null)
        {
            Debug.LogWarning("No data found, new game must be started before data can be saved");
            return;
        }

        foreach (ISaver thingToSave in thingsToSave)
        {
            thingToSave.SaveData(ref curCharData);
        }

        Debug.Log("GameSaved");

        serializer.Save(curCharData, selectedProfileId);
    }

    public void LoadCharacterData(string profileID)
    {

        this.curCharData = serializer.Load(selectedProfileId);

        if(this.curCharData == null && initilaizeDataIfNull)
        {
            NewCharacter("Dummy");
        }

        if (this.curCharData == null)
        {
            Debug.Log(":No data was found. A new game must be started before data can be loaded.");
            return;
        }

        foreach (ISaver thingToSave in thingsToSave)
        {
            thingToSave.LoadData(curCharData);
        }

        Debug.Log("GameLoaded");
    }

    private List<ISaver> FindAllItemsToSave()
    {
        IEnumerable<ISaver> itemsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISaver>();

        return new List<ISaver>(itemsToSave);
    }


    public void NewCharacter(string name)
    {
        // Assuming curCharData is a field of type CharacterData
        this.curCharData = new CharacterData
        {
            Name = name,
            ProfileId = name,
            Items = new List<WeaponStats>()
        };

        // Save the new character data under the new profile ID
        SaveDataAsync();

    }

    public string GetCurrentCharacterName()
    {
        if (curCharData != null)
        {
            return curCharData.Name;
        }
        else
        {
            return "";
        }
    }

    public bool IsProfileNameUnique(string profileName)
    {
        var allProfiles = GetAllProfileGameData();
        return !allProfiles.ContainsKey(profileName);
    }

    public bool HasCharData()
    {
        return curCharData != null;
    }

    public Dictionary<string, CharacterData> GetAllProfileGameData()
    {
        return serializer.LoadAllProfiles();
    }
}
