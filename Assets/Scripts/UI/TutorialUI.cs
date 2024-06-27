using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI interactAlternateText;
    [SerializeField] private TextMeshProUGUI escText;
    [SerializeField] private TextMeshProUGUI interactGamePadText;
    [SerializeField] private TextMeshProUGUI interactAlternateGamePadText;
    [SerializeField] private TextMeshProUGUI pauseGamePadText;
    
    [SerializeField] private Transform tutorialCanvas;
    private const string PLAYER_PREFS_TUTORIAL_AUTOSHOW = "TutorialAutoShow";
    private void Awake()
    {
        // if (PlayerPrefs.HasKey(PLAYER_PREFS_TUTORIAL_AUTOSHOW)== false)
        // {
        //     Show();
        //     PlayerPrefs.SetInt(PLAYER_PREFS_TUTORIAL_AUTOSHOW,0);
        //     
        // }
        // else
        // {
        //     Hide(this, EventArgs.Empty);
        // }
        Show();
        
    }

    private void Start()
    {
        GameInput.Instance.OnInteractAction += SetPlayerReady;
        GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
        UpdateVisual();
        KitchenGameManager.Instance.OnPlayerReady += KitchenGameManager_OnPlayerReady;
    }

    private void SetPlayerReady(object sender, EventArgs e)
    {
        KitchenGameManager.Instance.isLocalPlayerReady = true;
        
    }

    private void KitchenGameManager_OnPlayerReady(object sender, EventArgs e)
    {
        Hide(this, EventArgs.Empty);
        Debug.Log("hide");
    }

    private void GameInput_OnBindingRebind(object sender, EventArgs e)
    {
        UpdateVisual();
    }

    private void Hide(object sender, EventArgs e)
    {
        tutorialCanvas.gameObject.SetActive(false);
        
    }
    public void Show()
    {
        Debug.Log("show");
        tutorialCanvas.gameObject.SetActive(true);
    }

    private void UpdateVisual()
    {
        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        escText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        interactGamePadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_Interact);
        interactAlternateGamePadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.GamePad_InteractAlternate);
        pauseGamePadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);
    }
}
