using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ButtonRebindingController : MonoBehaviour
{
    [SerializeField] private Button MoveUp;
    [SerializeField] private Button MoveDown;
    [SerializeField] private Button MoveLeft;
    [SerializeField] private Button MoveRight;
    [SerializeField] private Button Interact;
    [SerializeField] private Button InteractAlternate;
    [SerializeField] private Button ESC;
    
    [SerializeField] private TextMeshProUGUI MoveUpText;
    [SerializeField] private TextMeshProUGUI MoveDownText;
    [SerializeField] private TextMeshProUGUI MoveLeftText;
    [SerializeField] private TextMeshProUGUI MoveRightText;
    [SerializeField] private TextMeshProUGUI InteractText;
    [SerializeField] private TextMeshProUGUI InteractAlternateText;
    [SerializeField] private TextMeshProUGUI ESCText;

    private void OnEnable()
    {
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        MoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        MoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        MoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        MoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        InteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        InteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
        ESCText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
       
    }

    
}
