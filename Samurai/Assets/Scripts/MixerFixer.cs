using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerFixer : MonoBehaviour
{

    public AudioMixer mixer;

    public AudioSource lightBGAudio;
    public AudioSource darkBGAudio;
    public AudioSource swordAudio;
    public AudioSource deathAudio;
    public AudioSource heavyAudio;
    public AudioSource dashAudio;

    void Start()
    {
        darkBGAudio.Play(0);
        darkBGAudio.Pause();
    }

    public void StartGame()
    {
        PlayLightBG();
    }

    public void SetMaster(float sliderValue)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusic(float sliderValue)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void SetEffects(float sliderValue)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(sliderValue) * 20);
    }

    public void PlayLightBG()
    {
        darkBGAudio.Pause();
        lightBGAudio.UnPause();
    }

    public void PlayDarkBG()
    {
        lightBGAudio.Pause();
        darkBGAudio.UnPause();
    }

    public void PlaySword1Audio()
    {
        swordAudio.PlayDelayed(0.1f);
    }

    public void PlayHeavyAudio()
    {
        heavyAudio.PlayDelayed(0f);
    }

    public void PlayDashAudio()
    {
        dashAudio.PlayDelayed(0f);
    }

    public void PlayDeathAudio()
    {
        deathAudio.PlayDelayed(0);
    }

}