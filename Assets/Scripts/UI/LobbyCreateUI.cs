using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInput;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button createPrivateButton;
    [SerializeField] private Button createPublicButton;

    private void Awake()
    {
        createPublicButton.onClick.AddListener((() => KitchenGameLobby.Instance.CreateLobby(lobbyNameInput.text, false)));
        createPrivateButton.onClick.AddListener((() => KitchenGameLobby.Instance.CreateLobby(lobbyNameInput.text, true)));
        closeButton.onClick.AddListener((() => Hide()));
    }

    private void Start()
    {
        Hide();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
