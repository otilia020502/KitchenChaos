using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Button closedButton;
    [SerializeField] private AudioSource backgroundMusic;

    private const string PLAYER_PREFS_MUSIC = "BackgroundMusicVolume";
    private const string PLAYER_PREFS_EFFECTS = "EffectsVolume";

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        effectsSlider.onValueChanged.AddListener(ChangeEffectsVolume);
        closedButton.onClick.AddListener(Hide);
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show()
    {
        musicSlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC, 1f);
        effectsSlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_EFFECTS, 1f);
        
        gameObject.SetActive(true);
        
    }

    private void ChangeEffectsVolume(float volume)
    {
        SoundManager.Instance.EffectsVolume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_EFFECTS,volume);
        PlayerPrefs.Save();
    }

    private void ChangeMusicVolume(float volume)
    {
        backgroundMusic.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC,volume);
        PlayerPrefs.Save();
    }
    
    
}
