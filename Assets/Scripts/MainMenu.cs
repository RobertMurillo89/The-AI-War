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

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
