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

public class SaveManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    private DataSerializer serializer;

    public static SaveManager Instance;
    private CharacterData curCharData;
    private float saveCooldown = 2f;
    private float lastSaveTime = -Mathf.Infinity;
    private List<ISaver> thingsToSave;

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
        }
    }
    #endregion

    private void Start()
    {
        this.serializer = new DataSerializer(Application.persistentDataPath, fileName, useEncryption);
        this.thingsToSave = FindAllItemsToSave();
        LoadCharacterData();

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
        foreach (ISaver thingToSave in thingsToSave)
        {
            thingToSave.SaveData(ref curCharData);
        }

        Debug.Log("GameSaved");

        serializer.Save(curCharData);
    }

    public void LoadCharacterData()
    {

        this.curCharData = serializer.Load();

        if (this.curCharData == null)
        {
            Debug.Log(":No data was found. Initializging data to defaults.");
            NewCharacter("");
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
        this.curCharData = new CharacterData
        {
            Name = name,
            Items = new List<WeaponStats>()
        };

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
}
