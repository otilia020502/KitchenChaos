using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsMenuButton;
    [SerializeField] private OptionsUI _optionsUI;
    private void Awake()
    {
        resumeButton.onClick.AddListener(() =>
        {
            KitchenGameManager.Instance.TogglePauseGame();
        } );
        mainMenuButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MenuScene);
        } );
        optionsMenuButton.onClick.AddListener(() =>
        {
            Hide();
           _optionsUI.Show(Show);
        } );
    }

    private void Start()
    {
        KitchenGameManager.Instance.OnLocalGamePaused += KitcenLocalGameManagerOnLocalGamePaused;
        KitchenGameManager.Instance.OnLocalGameUnPaused += KitcenLocalGameManagerOnLocalGameUnPaused;
        Hide();
    }

    private void KitcenLocalGameManagerOnLocalGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void KitcenLocalGameManagerOnLocalGamePaused(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
        
        resumeButton.Select();
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
