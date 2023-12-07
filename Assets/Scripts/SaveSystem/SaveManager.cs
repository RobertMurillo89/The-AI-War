using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEditor.Progress;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private CharacterData curCharData;
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
        if (Time.time - lastSaveTime > saveCooldown)
        {
            lastSaveTime = Time.time;
            SaveDataAsync();
        }
    }

    private void SaveDataAsync()
    {
        Task.Run(() =>
        {
            // Serialie currentPlayerData to binary
            //Write to file
        });
    }


    // other methods for loading data, hashing, etc...


   // Attach save requests to these events.For example, when a player picks up an item,
   // it should call SaveManager.Instance.RequestSave()
}
