using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel; // Main menu UI panel
    [SerializeField] private GameObject characterCreationPanel; // Character creation UI panel
    [SerializeField] private GameObject selectCharPanel; // Select character UI panel
    [SerializeField] private GameObject newCharPanel; // New character UI panel
    [SerializeField] private GameObject characterCreationOptions; // Options within the character creation panel
    [SerializeField] private GameObject selectCharacterOptions; // Options for selecting a character

    [Header("Character Creation")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu; // Menu for save slots in character creation
    [SerializeField] private TMP_InputField NameInputField; // Input field for entering character's name
    [SerializeField] private TMP_Text characterNameText; // Text field for displaying the character's name
    [SerializeField] private CharacterData characterData; // Data object for the current character

    [Header("Buttons")]
    [SerializeField] private Button playButton; // Button to start the game
    [SerializeField] private Button createCharacterButton; // Button for triggering character creation
    [SerializeField] private Button checkButton; // Button for checking (e.g., name availability)

    [Header("State Management")]
    [SerializeField] private string selectedProfileId; // ID of the currently selected profile

    // Singleton instance
    public static MainMenu Instance; // Singleton instance of MainMenu

    private void Awake() // The Awake method is called when the script instance is being loaded
    {
        Instance = this; // Assigning this instance to the static Instance field
        checkButton.gameObject.SetActive(false); // Deactivating the checkButton GameObject
        NameInputField.onValueChanged.AddListener(delegate { EnableCheckButton(); }); // Adding a listener to the onValueChanged event of the NameInputField
    }

    private void Start() // The Start method is called before the first frame update
    {
        if (!SaveManager.Instance.HasCharData()) // Checking if there is no character data
        {
            playButton.interactable = false; // Disabling the playButton
        }
        else // If there is character data
        {
            UpdateCharacterNameOnUI(); // Updating the character name on the UI
            saveSlotsMenu.ActivateMenu(); // Creating a new save slot
        }
    }


    #region ButtonFunctions

    public void PlayGame() // Method for starting the game
    {
        SceneManager.LoadSceneAsync(1); // Loading the scene with index 1 asynchronously
    }

    public void SelectCharacterMenu() // Method for opening the character selection menu
    {
        saveSlotsMenu.ActivateMenu(); // Activating the save slots menu

        if (SaveManager.Instance.HasCharData()) // Checking if there is character data
        {
            selectCharPanel.SetActive(true); // Activating the selectCharPanel
            newCharPanel.SetActive(false); // Deactivating the newCharPanel
        }
        else // If there is no character data
        {
            newCharPanel.SetActive(true); // Activating the newCharPanel
            selectCharPanel.SetActive(false); // Deactivating the selectCharPanel
        }
    }

    public void ActivateNameEntry() // Method for activating the name entry field
    {
        NameInputField.gameObject.SetActive(true); // Activating the NameInputField GameObject
        NameInputField.Select(); // Selecting the NameInputField
        NameInputField.ActivateInputField(); // Activating the input field of the NameInputField
    }

    private void EnableCheckButton() // Method for enabling the check button
    {
        string inputName = NameInputField.text; // Getting the text from the NameInputField
        string name = SaveManager.Instance.GetCurrentCharacterName(); // Getting the current character name from the SaveManager

        checkButton.gameObject.SetActive(inputName.Length > 0 && name != inputName); // Activating the checkButton GameObject if the input name is not empty and different from the current character name

        //if (inputName.Length > 0 && name == inputName) // If the input name is not empty and the same as the current character name
        //{
        //    // Update UI to show that the name is not unique
        //}
    }

    public void CreateCharacter() // Method for creating a character
    {
        string newCharacterName = NameInputField.text; // Getting the text from the NameInputField
        SaveManager.Instance.NewCharacter(newCharacterName); // Creating a new character with the new character name
        saveSlotsMenu.CreateSaveSlot(newCharacterName);
        characterCreationOptions.gameObject.SetActive(false); // Deactivating the characterCreationOptions GameObject
        selectCharacterOptions.gameObject.SetActive(true); // Activating the selectCharacterOptions GameObject
        UpdateCharacterNameOnUI(); // Updating the character name on the UI
    }

    public void SetSelectedProfileId(string id) // Method for setting the selected profile ID
    {
        selectedProfileId = id; // Assigning the id to the selectedProfileId
        CustomLogger.Log("Selected Profile ID: " + selectedProfileId); // Logging the selected profile ID
    }

    public void SelectCharacterAndReturnToMenu() // Method for selecting a character and returning to the menu
    {
        if (!string.IsNullOrEmpty(this.selectedProfileId)) // Checking if the selectedProfileId is not null or empty
        {
            saveSlotsMenu.ActivateMenu(); // Activating the save slots menu
            SaveManager.Instance.SetSelectedProfile(this.selectedProfileId); // Setting the selected profile in the SaveManager
            SaveManager.Instance.LoadCharacterData(this.selectedProfileId); // Loading the character data of the selected profile
            UpdateCharacterNameOnUI(); // Updating the character name on the UI
        }
        else // If the selectedProfileId is null or empty
        {
            CustomLogger.Log("wtf"); // Logging "wtf"
        }

        characterCreationPanel.gameObject.SetActive(false); // Deactivating the characterCreationPanel GameObject
        mainMenuPanel.gameObject.SetActive(true); // Activating the mainMenuPanel GameObject

        characterNameText.text = SaveManager.Instance.GetCurrentCharacterName(); // Setting the text of the characterNameText to the current character name

        playButton.interactable = true; // Enabling the playButton
    }

    public void SwitchCharacter() // Method for switching characters
    {
        if (newCharPanel.activeSelf) // Checking if the newCharPanel is active
        {
            selectCharPanel.SetActive(true); // Activating the selectCharPanel
            newCharPanel.SetActive(false); // Deactivating the newCharPanel
        }
        else // If the newCharPanel is not active
        {
            newCharPanel.SetActive(true); // Activating the newCharPanel
            selectCharPanel.SetActive(false); // Deactivating the selectCharPanel
        }

        UpdateCharacterNameOnUI(); // Updating the character name on the UI
    }

    private void UpdateCharacterNameOnUI() // Method for updating the character name on the UI
    {
        if (SaveManager.Instance != null) // Checking if the SaveManager instance is not null
        {
            string currentCharacterName = SaveManager.Instance.GetCurrentCharacterName(); // Getting the current character name from the SaveManager
            characterNameText.text = currentCharacterName; // Setting the text of the characterNameText to the current character name
        }
    }

    public void QuitGame() // Method for quitting the game
    {
        Application.Quit(); // Quitting the application
    }
    #endregion
}
