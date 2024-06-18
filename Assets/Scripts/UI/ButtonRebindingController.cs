using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ButtonRebindingController : MonoBehaviour
{
    [FormerlySerializedAs("MoveUp")] [SerializeField] private Button moveUp;
    [FormerlySerializedAs("MoveDown")] [SerializeField] private Button moveDown;
    [FormerlySerializedAs("MoveLeft")] [SerializeField] private Button moveLeft;
    [FormerlySerializedAs("MoveRight")] [SerializeField] private Button moveRight;
    [FormerlySerializedAs("Interact")] [SerializeField] private Button interact;
    [FormerlySerializedAs("InteractAlternate")] [SerializeField] private Button interactAlternate;
    [FormerlySerializedAs("ESC")] [SerializeField] private Button esc;
    [SerializeField] private Button interactGamePad;
    [SerializeField] private Button interactAlternateGamePad;
    [SerializeField] private Button escGamePad;
    
    [FormerlySerializedAs("MoveUpText")] [SerializeField] private TextMeshProUGUI moveUpText;
    [FormerlySerializedAs("MoveDownText")] [SerializeField] private TextMeshProUGUI moveDownText;
    [FormerlySerializedAs("MoveLeftText")] [SerializeField] private TextMeshProUGUI moveLeftText;
    [FormerlySerializedAs("MoveRightText")] [SerializeField] private TextMeshProUGUI moveRightText;
    [FormerlySerializedAs("InteractText")] [SerializeField] private TextMeshProUGUI interactText;
    [FormerlySerializedAs("InteractAlternateText")] [SerializeField] private TextMeshProUGUI interactAlternateText;
    [FormerlySerializedAs("ESCText")] [SerializeField] private TextMeshProUGUI escText;
    [SerializeField] private TextMeshProUGUI interactGamePadText;
    [SerializeField] private TextMeshProUGUI interactAlternateGamePadText;
    [SerializeField] private TextMeshProUGUI escGamePadText;
    [SerializeField] private Transform pressToRebindKeyTransform;
    
    private void Awake()
    {
        moveUp.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveUp);});
        moveDown.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveDown);});
        moveRight.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveRight);});
        moveLeft.onClick.AddListener(() => { RebindBinding(GameInput.Binding.MoveLeft);});
        interact.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Interact);});
        interactAlternate.onClick.AddListener(() => { RebindBinding(GameInput.Binding.InteractAlternate);});
        esc.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Pause);});
        interactGamePad.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_Interact);});
        interactAlternateGamePad.onClick.AddListener(() => { RebindBinding(GameInput.Binding.GamePad_InteractAlternate);});
        escGamePad.onClick.AddListener(() => { RebindBinding(GameInput.Binding.Gamepad_Pause);});
    }

    private void Start()
    {
        HidePressToRebindKey();
    }

    private void OnEnable()
    {
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
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
        escGamePadText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Gamepad_Pause);

    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
        
        
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
        GameInput.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisuals();
        });
    }
}
