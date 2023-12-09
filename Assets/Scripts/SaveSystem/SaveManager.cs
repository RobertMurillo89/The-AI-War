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

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public CharacterData curCharData;
    private float saveCooldown = 5f;
    private float lastSaveTime = -Mathf.Infinity;

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

    public void RequestSave()
    {
        if(curCharData.NeedSave())
        {
            if (Time.time - lastSaveTime > saveCooldown)
            {
                lastSaveTime = Time.time;
                SaveDataAsync();
                curCharData.ResetSaveFlag();
            }
        }

    }

    private void SaveDataAsync()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "characterData.dat");

        Task.Run(() =>
        {
            // Assuming curCharData is the current instance of CharacterData to be saved
            DataSerializer.SerializeObject(filePath, curCharData);
            Debug.Log(filePath);
            // You can also handle exceptions here to deal with any serialization errors
        });
    }

    public void LoadCharacterData()
    {
        if (this.curCharData == null)
        {
            Debug.Log(":No data was found. Initializging data to defaults.");
            NewCharacter();
        }
        string filePath = Path.Combine(Application.persistentDataPath, "characterData.dat");
        curCharData = DataSerializer.DeserializeObject<CharacterData>(filePath);
    }

    public void NewCharacter()
    {
        this.curCharData = new CharacterData();
    }
    // other methods for hashing, etc...


   // Attach save requests to these events. For example, when a player picks up an item,
   // it should call SaveManager.Instance.RequestSave()
}
