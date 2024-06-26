using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    public AudioMixer AudioMixer;
    public AudioSource MusicSource, AmbianceSource, SFXSource;
    public Slider MasterSlider, MusicSlider, AmbiSlider, SFXSlider;
    public AudioClip[] MainMenuMusic, MainMenuAmbi;
    private float masterVolume, musicVolume, ambiVolume, sfxVolume;
    private bool isMasterMuted, isMusicMuted, isAmbiMuted, isSFXMuted;

    private void Start()
    {
        UpdateInitialVolumes();
        PlayMusic(MusicSource, MainMenuMusic);
        PlayAmbiance(AmbianceSource, MainMenuAmbi);
    }

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
        if (isMuted)
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
        AudioMixer.SetFloat("MasterVolume", Mathf.Log10(value / 100) * 20f);
    }
    public void MusicVolume(float value)
    {
        MusicSlider.value = value;
        musicVolume = value;
        AudioMixer.SetFloat("MusicVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void AmbiVolume(float value)
    {
        AmbiSlider.value = value;
        ambiVolume = value;
        AudioMixer.SetFloat("AmbianceVolume", Mathf.Log10(value / 100) * 20f);
    }

    public void SFXVolume(float value)
    {
        SFXSlider.value = value;
        sfxVolume = value;
        AudioMixer.SetFloat("SFXVolume", Mathf.Log10(value / 100) * 20f);
    }

    #endregion
}
