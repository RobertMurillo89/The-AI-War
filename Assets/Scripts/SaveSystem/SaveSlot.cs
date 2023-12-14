using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class SaveSlot : MonoBehaviour
{
    // Serialized field to assign a unique identifier for each save slot
    [Header("Profile")]
    [SerializeField] private string profileId = "";
    // Reference to the GameObject that is shown when there is no data for this slot
    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    // Reference to the GameObject that is shown when this slot has data
    [SerializeField] private GameObject hasDataContent;
    // Text component for displaying the character's name associated with this slot
    [SerializeField] private TextMeshProUGUI characterName;

    // Method to update the slot's display based on the given CharacterData
    public void SetData(CharacterData characterData)
    {
        // If there is no character data (null), show 'no data' content and hide 'has data' content
        if (characterData == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            // If there is character data, show 'has data' content and hide 'no data' content
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);

            // Update the text to show the character's name
            characterName.text = characterData.Name;
        }
    }

    // Method called when this save slot is clicked
    public void OnClickSaveSlot()
    {
        // Notify the MainMenu to update the selected profile ID with this slot's profile ID
        MainMenu.Instance.SetSelectedProfileId(this.profileId);
    }

    // Getter method to access the private profileId field
    public string GetProfileId()
    {
        return this.profileId;
    }

    public void SetProfileId(string id)
    {
        profileId = id;
    }

}
