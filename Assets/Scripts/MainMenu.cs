using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public AudioMixer AudioMixer;
    public AudioSource MusicSource, AmbianceSource, SFXSource;
    public AudioClip[] MainMenuMusic, MainMenuAmbi;
    public Slider MasterSlider, MusicSlider, AmbiSlider, SFXSlider, MouseSensitivity;

    private void Start()
    {

        PlayMusic(MusicSource, MainMenuMusic);

    }
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
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

    }

    public void ToggleMusic()
    {

    }

    public void ToggleAmbi()
    {

    }

    public void ToggleSFX()
    {

    }
    public void MasterVolume(float value)
    {
        MasterSlider.value = value;
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain
    }
    public void MusicVolume(float value)
    {
        MusicSlider.value = value;
        AudioMixer.SetFloat("MusicVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain

    }

    public void AmbiVolume(float value)
    {
        AmbiSlider.value = value;
        AudioMixer.SetFloat("AmbianceVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain

    }

    public void SFXVolume(float value)
    {
        SFXSlider.value = value;
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(value / 100) * 20f); // need chad to explain

    }
}
