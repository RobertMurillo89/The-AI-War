using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotsMenu : MonoBehaviour
{
    [SerializeField] private GameObject saveSlotPrefab; // Assign in Unity Editor
    [SerializeField] private Transform saveSlotContainer; // Assign in Unity Editor
    private SaveSlot[] saveSlots;

    private void Awake()
    {
        saveSlots = this.GetComponentsInChildren<SaveSlot>();
    }

    private void Start()
    {
        // Get all save profiles from SaveManager
        Dictionary<string, CharacterData> saveProfiles = SaveManager.Instance.GetAllProfileGameData();

        // Create a save slot for each save profile
        // Create a save slot for each save profile
    foreach (KeyValuePair<string, CharacterData> profile in saveProfiles)
        {
            CreateSaveSlot(profile.Key);
        }

        ActivateMenu();
    }

    // Activates and populates the menu with data for each save slot,
    // updating each slot with corresponding character data if available
    public void ActivateMenu()
    {
        // Retrieve a dictionary of all profiles and their corresponding character data from SaveManager
        Dictionary<string, CharacterData> profilesCharacterData = SaveManager.Instance.GetAllProfileGameData();

        // Iterate over each save slot present in the menu
        foreach (SaveSlot saveSlot in saveSlots)
        {
            // Initialize a variable to hold the character data for the current save slot
            CharacterData profileData = null;

            // Try to get the character data associated with the current save slot's profile ID.
            // If successful, profileData is set; if not, profileData remains null
            profilesCharacterData.TryGetValue(saveSlot.GetProfileId(), out profileData);

            // Update the current save slot with the retrieved or default character data
            saveSlot.SetData(profileData);
        }
    }
    public void CreateSaveSlot(string profileId)
    {
        // Instantiate a new save slot from the prefab
        GameObject newSlot = Instantiate(saveSlotPrefab, saveSlotContainer);
        // Assign a unique profile ID to the new save slot
        SaveSlot saveSlotScript = newSlot.GetComponent<SaveSlot>();
        saveSlotScript.SetProfileId(profileId); // Assign the profile ID
        saveSlots = this.GetComponentsInChildren<SaveSlot>(); // Update the saveSlots array
    }
}
