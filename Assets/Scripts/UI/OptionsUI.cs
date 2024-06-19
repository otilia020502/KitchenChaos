using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    [SerializeField] private Button closedButton;
    [SerializeField] private Button showTutorialButton;
    [SerializeField] private VolumeController _volumeController;
    [SerializeField] private TutorialUI _tutorialUI;
    private Action onClosedButtonAction;

    
    
    private void Start()
    {
        closedButton.onClick.AddListener(() =>
        {
            Hide();
            onClosedButtonAction();
        });
        Hide();
        showTutorialButton.onClick.AddListener(ShowTutorialWindow);
    }

    private void ShowTutorialWindow()
    {
        _tutorialUI.Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    public void Show(Action onClosedButtonAction)
    {
        this.onClosedButtonAction = onClosedButtonAction;
        
        _volumeController.UpdateSliders();
        gameObject.SetActive(true);
        
        closedButton.Select();
        
    }
}
