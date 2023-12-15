using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using System;

public class SaveManager : SingletonMonoBehaviour<SaveManager>
{
    // Debugging Tool, If true, data will initialize if null.Remove from final build
    [Header("Debugging")]
    [SerializeField] private bool initilaizeDataIfNull = false;

    [Header("File Storage Config")]
    // Serialized field for file name configuration
    [SerializeField] private string fileName;
    // Serialized field to determine whether to use encryption
    [SerializeField] private bool useEncryption;

    [Header("Save Cooldown")]
    // Cooldown duration for saving data
    [SerializeField] float saveCooldown = 2f;
    // Time since last save initialized to negative infinity
    private float lastSaveTime = -Mathf.Infinity;

    [Header("Data Management")]
    // Private field for a DataSerializer object
    private DataSerializer serializer;
    // Private field to store current character data
    private CharacterData curCharData;
    // String to store the selected profile ID
    private string selectedProfileId = "";

    [Header("Savable Objects")]
    // List of objects implementing ISaver interface to be saved
    private List<ISaver> thingsToSave;

    protected override void Awake()
    {
        try
        {
            this.serializer = new DataSerializer(Application.persistentDataPath, fileName, useEncryption);
        }
        catch (Exception ex)
        {
            CustomLogger.LogError("Error initializing DataSerializer: " + ex.Message);
            // Handle the initialization failure, maybe disable save/load features
        }
    }


    #region Scene Load/Unload
    // Subscribe/unscubscribe to the sceneLoaded/Unloaded event
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
        // Find all items to save in the loaded scene
        this.thingsToSave = FindAllItemsToSave();
        // Load character data for the selected profile
        LoadCharacterData(selectedProfileId);
    }

    public void OnSceneUnloaded(Scene scene)
    {
        // save called when a scene is unloaded.
        _ = SaveDataAsync();
    }
    #endregion

    public void RequestSave()
    {
        // Check if enough time has passed since the last save
        if (Time.time - lastSaveTime > saveCooldown)
        {
            // Update the last save time to current time
            lastSaveTime = Time.time;
            var _ = SaveDataAsync(); // Asynchronously saving without awaiting
        }

    }

    public async Task SaveDataAsync()
    {
        // If current character data is null
        if (this.curCharData == null)
        {
            CustomLogger.LogWarning("No data found, new game must be started before data can be saved");
            return;
        }
        // Perform the save operation asynchronously
        await Task.Run(() =>
        {
            // Iterate over each object to be saved
            foreach (ISaver thingToSave in thingsToSave)
            {
                // Call SaveData method on each object, passing the current character data by reference
                thingToSave.SaveData(ref curCharData);
            }
            // Call the serializer's save method with current character data and selected profile ID
            serializer.Save(curCharData, selectedProfileId);
        });

        CustomLogger.Log("GameSaved");

    }

    public void LoadCharacterData(string profileID)
    {
        CustomLogger.Log("Loading Character Data for profile: " + profileID);

        try
        {
            // Load character data using the serializer
            this.curCharData = serializer.Load(profileID);
            // Rest of the code...
        }
        catch (Exception ex)
        {
            CustomLogger.LogError("Error loading character data: " + ex.Message);
            // Handle loading failure
        }

        // If loaded data is null and initialization is allowed
        if (this.curCharData == null && initilaizeDataIfNull)
            {
                // Create a new character with a dummy name
                NewCharacter("Dummy");
            }

        if (this.curCharData == null)
            {
                // If loaded data is still null
                CustomLogger.Log(":No data was found. A new game must be started before data can be loaded.");
                return;
            }

        // Iterate over each object to be loaded
        foreach (ISaver thingToSave in thingsToSave)
            {
            // Call LoadData method on each object with current character data
            thingToSave.LoadData(curCharData);
            }

        CustomLogger.Log("GameLoaded");
    }

    private List<ISaver> FindAllItemsToSave()
    {
        try
        {
            // Find all MonoBehaviour objects that implement ISaver interface
            IEnumerable<ISaver> itemsToSave = FindObjectsOfType<MonoBehaviour>().OfType<ISaver>();
            // Return a list of these objects
            return new List<ISaver>(itemsToSave);
        }
        catch (Exception ex)
        {
            CustomLogger.LogError("Error finding items to save: " + ex.Message);
            return new List<ISaver>(); // Return an empty list or handle accordingly
        }
    }

    public void NewCharacter(string name)
    {
        // Instantiate a new CharacterData object
        this.curCharData = new CharacterData
        {
            Name = name,
            ProfileId = name,
            Items = new List<WeaponStats>()
        };
        selectedProfileId = name;
        // Save the new character data under the new profile ID
        var _ = SaveDataAsync(); // Asynchronously saving without awaiting

    }

    // Public method to get the current character's name
    public string GetCurrentCharacterName()
    {
        if (curCharData != null)
        {
            return curCharData.Name;
        }
        else
        {
            CustomLogger.Log("could not find char name");
            return "";
        }
    }

    // Public method to check if character data exists
    public bool HasCharData()
    {
        // Return true if current character data is not null
        return curCharData != null;
    }

    // Public method to get all profile game data
    public Dictionary<string, CharacterData> GetAllProfileGameData()
    {
        // Return all profiles using the serializer's LoadAllProfiles method
        return serializer.LoadAllProfiles();
    }

    public void SetSelectedProfile(string profileID)
    {
        selectedProfileId = profileID;
    }
}
