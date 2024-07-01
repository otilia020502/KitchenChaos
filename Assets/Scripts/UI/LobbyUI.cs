using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quickJoinButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private LobbyCreateUI _lobbyCreateUI;
    [SerializeField] private Button joinCodeButton;
    [SerializeField] private TMP_InputField joinCodeInput;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener((() => Loader.Load(Loader.Scene.MyMenuScene)));
        quickJoinButton.onClick.AddListener((() => KitchenGameLobby.Instance.QuickJoin()));
        createLobbyButton.onClick.AddListener((() => _lobbyCreateUI.Show()));
        joinCodeButton.onClick.AddListener((() => KitchenGameLobby.Instance.JoinWithCode(joinCodeInput.text)));
    }
    
}
