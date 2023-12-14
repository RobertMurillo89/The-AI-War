using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [Header("----- Audio Components -----")]
    public AudioMixer AudioMixer;
    public AudioSource MusicSource, AmbianceSource, SFXSource;
    public Slider MasterSlider, MusicSlider, AmbiSlider, SFXSlider;
    public AudioClip[] MainMenuMusic, MainMenuAmbi;
    private float masterVolume, musicVolume, ambiVolume, sfxVolume;
    private bool isMasterMuted, isMusicMuted, isAmbiMuted, isSFXMuted;

    [Header("-----Charcter Creation-----")]
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
        // Ensures inputfield for Char Creation is not empty
        checkButton.gameObject.SetActive(false);
        NameInputField.onValueChanged.AddListener(delegate { EnableCheckButton(); });
    }
    private void Start()
    {
        // Disables Play button if no Char data loaded. 
        if (!SaveManager.Instance.HasCharData())
        {
            playButton.interactable = false;
        }
        else
        {
            // Shows which char is active on mm ui.
            UpdateCharacterNameOnUI();
        }
        //Handles audio
        UpdateInitialVolumes();
        PlayMusic(MusicSource, MainMenuMusic);

        

    }

    #region ButtonFunctions

    //Play button function loads next scene if character data loaded.
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);       
    }

    // Handles the logic for displaying the appropriate character menu based on the presence of saved char data
    public void SelectCharacterMenu()
    {
        // Update and populate the save slots menu with the latest data
        saveSlotsMenu.ActivateMenu();

        // Check if there is any saved character data available
        if (SaveManager.Instance.HasCharData())
            {
                // If a save profile exists, open the select character panel
                selectCharPanel.SetActive(true);
                newCharPanel.SetActive(false);
            }
            else
            {
                // If no save profile exists, open the new character panel
                newCharPanel.SetActive(true);
                selectCharPanel.SetActive(false);
            }            
        //this.DeactivateMenu();
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

        // Enable the check button if there's text and it's a unique name
        checkButton.gameObject.SetActive(inputName.Length > 0 && name != inputName);

        // Optionally, provide immediate feedback if the name is not unique
        if (inputName.Length > 0 && name == inputName)
        {
            // Update UI to show that the name is not unique
            // e.g., a warning text, change the color of the input field, etc.
        }
    }
    public void CreateCharacter()
    {
        saveSlotsMenu.CreateSaveSlot();
        string newCharacterName = NameInputField.text;

        // Generate a new profile ID for this character
        string newProfileId = newCharacterName;

        // NewCharacter now returns the new profile ID
        SaveManager.Instance.NewCharacter(newCharacterName);
        SaveManager.Instance.RequestSave();

        // Update the main menu with the selected profile ID
        SetSelectedProfileId(newProfileId);

        // Deactivate the creation options and show the select character options
        characterCreationOptions.gameObject.SetActive(false);
        selectCharacterOptions.gameObject.SetActive(true);

        // Update the save slots to include the new character
        //saveSlotsMenu.UpdateSaveSlots();
    }

    public void SetSelectedProfileId(string id)
    {
        selectedProfileId = id;
    }

    public void SelectCharacterAndReturnToMenu()
    {

        // Load the selected character data
        if (!string.IsNullOrEmpty(this.selectedProfileId))
        {
            saveSlotsMenu.ActivateMenu();
            SaveManager.Instance.LoadCharacterData(this.selectedProfileId);
            UpdateCharacterNameOnUI();
        }

        characterCreationPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);

        characterNameText.text = SaveManager.Instance.GetCurrentCharacterName();
        UpdateCharacterNameOnUI();

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

    #region Audio

    private void UpdateInitialVolumes()
    {
        masterVolume = MasterSlider.value;
        musicVolume = MusicSlider.value;
        ambiVolume = AmbiSlider.value;
        sfxVolume = SFXSlider.value;
    }
    void PlayMusic(AudioSource source, AudioClip[] clips)
    {
        AudioFunctionalities.PlayRandomClip(source, clips);
    }

    void PlayAmbiance(AudioSource source, AudioClip[] clips)
    {
        AudioFunctionalities.PlayRandomClip(source, clips);
    }

    public void PlayButtonSound(AudioClip clip)
    {
        SFXSource.clip = clip;
        SFXSource.Play();
    }

    public void ToggleMaster()
    {
        ToggleAudio("MasterVolume", ref isMasterMuted, ref masterVolume, MasterSlider);

    }

    public void ToggleMusic()
    {
        ToggleAudio("MusicVolume", ref isMusicMuted, ref musicVolume, MusicSlider);
    }

    public void ToggleAmbi()
    {
        ToggleAudio("AmbianceVolume", ref isAmbiMuted, ref ambiVolume, AmbiSlider);
    }

    public void ToggleSFX()
    {
        ToggleAudio("SFXVolume", ref isSFXMuted, ref sfxVolume, SFXSlider);
    }

    private void ToggleAudio(string mixerParam, ref bool isMuted, ref float currentVolume, Slider slider)
    {
        if(isMuted)
        {
            AudioMixer.SetFloat(mixerParam, Mathf.Log10(currentVolume / 100) * 20f);
        }
        else
        {
            AudioMixer.GetFloat(mixerParam, out currentVolume);
            currentVolume = Mathf.Pow(10, currentVolume / 20) * 100;
            AudioMixer.SetFloat(mixerParam, -80f); //mute
        }
        isMuted = !isMuted;
        slider.interactable = !isMuted; //optionally disable slider when volume is muted
    }
    public void MasterVolume(float value)
    {
        MasterSlider.value = value;
        masterVolume = value;
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain
    }
    public void MusicVolume(float value)
    {
        MusicSlider.value = value;
        musicVolume = value;
        AudioMixer.SetFloat("MusicVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain

    }

    public void AmbiVolume(float value)
    {
        AmbiSlider.value = value;
        ambiVolume = value;
        AudioMixer.SetFloat("AmbianceVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain

    }

    public void SFXVolume(float value)
    {
        SFXSlider.value = value;
        sfxVolume = value;
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain

    }

    #endregion
}
