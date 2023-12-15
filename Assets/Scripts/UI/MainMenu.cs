using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("-----Character Creation-----")]
    [SerializeField] private SaveSlotsMenu saveSlotsMenu;
    public Button createCharacterButton;
    public TMP_InputField NameInputField;
    public Button checkButton;
    public GameObject selectCharacterOptions;
    public GameObject characterCreationPanel;
    public GameObject mainMenuPanel;
    public CharacterData characterData;
    public TMP_Text characterNameText;
    public GameObject characterCreationOptions;
    [SerializeField] private Button playButton;
    public GameObject newCharPanel;
    public GameObject selectCharPanel;

    private string selectedProfileId;

    public static MainMenu Instance;
    private void Awake()
    {
        Instance = this;
        checkButton.gameObject.SetActive(false);
        NameInputField.onValueChanged.AddListener(delegate { EnableCheckButton(); });
    }
    private void Start()
    {
        if (!SaveManager.Instance.HasCharData())
        {
            playButton.interactable = false;
        }
        else
        {
            UpdateCharacterNameOnUI();
        }
    }

    #region ButtonFunctions

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void SelectCharacterMenu()
    {
        saveSlotsMenu.ActivateMenu();

        if (SaveManager.Instance.HasCharData())
        {
            selectCharPanel.SetActive(true);
            newCharPanel.SetActive(false);
        }
        else
        {
            newCharPanel.SetActive(true);
            selectCharPanel.SetActive(false);
        }
    }

    public void ActivateNameEntry()
    {
        NameInputField.gameObject.SetActive(true);
        NameInputField.Select();
        NameInputField.ActivateInputField();
    }

    private void EnableCheckButton()
    {
        string inputName = NameInputField.text;
        string name = SaveManager.Instance.GetCurrentCharacterName();

        checkButton.gameObject.SetActive(inputName.Length > 0 && name != inputName);

        if (inputName.Length > 0 && name == inputName)
        {
            // Update UI to show that the name is not unique
        }
    }
    public void CreateCharacter()
    {
        saveSlotsMenu.CreateSaveSlot();
        string newCharacterName = NameInputField.text;

        string newProfileId = newCharacterName;

        SaveManager.Instance.NewCharacter(newCharacterName);
        SaveManager.Instance.RequestSave();

        characterCreationOptions.gameObject.SetActive(false);
        selectCharacterOptions.gameObject.SetActive(true);
        UpdateCharacterNameOnUI();
    }

    public void SetSelectedProfileId(string id)
    {
        selectedProfileId = id;
        CustomLogger.Log("Selected Profile ID: " + selectedProfileId);
    }

    public void SelectCharacterAndReturnToMenu()
    {
        if (!string.IsNullOrEmpty(this.selectedProfileId))
        {
            saveSlotsMenu.ActivateMenu();
            SaveManager.Instance.SetSelectedProfile(this.selectedProfileId);
            SaveManager.Instance.LoadCharacterData(this.selectedProfileId);
            UpdateCharacterNameOnUI();
        }
        else
        {
            CustomLogger.Log("wtf");
        }

        characterCreationPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);

        characterNameText.text = SaveManager.Instance.GetCurrentCharacterName();

        playButton.interactable = true;
    }

    public void SwitchCharacter()
    {
        if (newCharPanel.activeSelf)
        {
            selectCharPanel.SetActive(true);
            newCharPanel.SetActive(false);
        }
        else
        {
            newCharPanel.SetActive(true);
            selectCharPanel.SetActive(false);
        }

        UpdateCharacterNameOnUI();
    }

    private void UpdateCharacterNameOnUI()
    {
        if (SaveManager.Instance != null)
        {
            string currentCharacterName = SaveManager.Instance.GetCurrentCharacterName();
            characterNameText.text = currentCharacterName;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
}
