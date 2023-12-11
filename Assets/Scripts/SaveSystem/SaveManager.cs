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

        //string filePath = Path.Combine(Application.persistentDataPath, "characterData.dat");
        //Task.Run(() =>
        //{
        //    // Assuming curCharData is the current instance of CharacterData to be saved
        //    DataSerializer.SerializeObject(filePath, curCharData);
        //    Debug.Log(filePath);
        //    // You can also handle exceptions here to deal with any serialization errors
        //});

        Debug.Log("GameSaved");
    }

    public void LoadCharacterData()
    {
        if (this.curCharData == null)
        {
            Debug.Log(":No data was found. Initializging data to defaults.");
            NewCharacter();
        }

        foreach (ISaver thingToSave in thingsToSave)
        {
            thingToSave.LoadData(curCharData);
        }

        //string filePath = Path.Combine(Application.persistentDataPath, "characterData.dat");
        //curCharData = DataSerializer.DeserializeObject<CharacterData>(filePath);

        Debug.Log("GameLoaded");
    }

    public void NewCharacter()
    {
        this.curCharData = new CharacterData();
    }

    private List<ISaver> FindAllItemsToSave()
    {
        IEnumerable<ISaver> itemsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISaver>();

        return new List<ISaver>(itemsToSave);
    }
}
