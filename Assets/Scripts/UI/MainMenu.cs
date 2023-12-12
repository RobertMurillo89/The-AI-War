using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    [Header("----- Audio Components -----")]
    public AudioMixer AudioMixer;
    public AudioSource MusicSource, AmbianceSource, SFXSource;
    public Slider MasterSlider, MusicSlider, AmbiSlider, SFXSlider;
    public AudioClip[] MainMenuMusic, MainMenuAmbi;

    [Header("-----Charcter Creation-----")]
    public Button createCharacterButton;
    public TMP_InputField NameInputField;
    public Button checkButton;  
    public GameObject selectCharacterOptions;
    public GameObject characterCreationPanel;
    public GameObject mainMenuPanel;
    public CharacterData characterData;
    public TMP_Text characterNameText;
    public GameObject characterCreationOptions;

    private float masterVolume, musicVolume, ambiVolume, sfxVolume;
    private bool isMasterMuted, isMusicMuted, isAmbiMuted, isSFXMuted;

    private void Awake()
    {
        NameInputField.gameObject.SetActive(false);
        checkButton.gameObject.SetActive(false);

        NameInputField.onValueChanged.AddListener(delegate { EnableCheckButton(); });

    }
    private void Start()
    {
        if (!SaveManager.Instance.HasCharData())
        {
            
        }
        UpdateInitialVolumes();
        PlayMusic(MusicSource, MainMenuMusic);
        UpdateCharacterNameOnUI();

    }

    #region ButtonFunctions
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);       
    }

    public void ActivateNameEntry()
    {
        NameInputField.gameObject.SetActive(true);
        NameInputField.Select();
        NameInputField.ActivateInputField();      
    }
    private void EnableCheckButton()
    {
        checkButton.gameObject.SetActive(NameInputField.text.Length > 0);

    }
    public void CreateCharacter()
    {
        Debug.Log("CreateCharacter Called.");

        string newCharacterName = NameInputField.text;
        SaveManager.Instance.NewCharacter(newCharacterName);
        SaveManager.Instance.RequestSave();

        characterCreationOptions.gameObject.SetActive(false);
        selectCharacterOptions.gameObject.SetActive(true);

        
      
    }

    public void SelectCharacterAndReturnToMenu()
    {
        characterCreationPanel.gameObject.SetActive(false);
        mainMenuPanel.gameObject.SetActive(true);
        characterNameText.text = SaveManager.Instance.GetCurrentCharacterName();
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
