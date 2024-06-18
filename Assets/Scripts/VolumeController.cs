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
        musicSlider.value = backgroundMusic.volume;
        effectsSlider.value = SoundManager.Instance.EffectsVolume;
        
        gameObject.SetActive(true);
        
    }

    private void ChangeEffectsVolume(float volume)
    {
        SoundManager.Instance.EffectsVolume = volume;
    }

    private void ChangeMusicVolume(float volume)
    {
        backgroundMusic.volume = volume;
        
    }
    
    
}
