using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioSource MusicSource;
    public AudioSource AmbianceSource;
    public AudioSource SFXSource;
    public AudioClip[] MainMenuMusic;
    public AudioClip[] MainMenuAmbi;
    public AudioClip[] MainMenuSFX;

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
    public void PlayButtonSound(AudioSource source, AudioClip[] clips)
    {
        AudioFunctionalities.PlayRandomClip(source, clips);
    }

    public void MasterVolume()
    {

    }
    public void MusicVolume(float volume)
    {
        MusicSource.volume = volume;
    }

    public void AmbiVolume(float volume)
    {
        AmbianceSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }
}
