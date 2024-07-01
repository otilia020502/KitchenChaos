using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class TestingCharacterSceneUI : MonoBehaviour
{
    [SerializeField] private Button readyButton;
    [SerializeField] private Button mainButton;
    [SerializeField] private TextMeshProUGUI lobbyName;
    [SerializeField] private TextMeshProUGUI lobbyCode;
    private void Awake()
    {
        readyButton.onClick.AddListener((() => { CharacterSelectReady.Instance.SetPlayerReady(); }));
        
    }

    private void Start()
    {
        Lobby lobby = KitchenGameLobby.Instance.GetLobby();
        lobbyName.text = "Lobby Name: "+lobby.Name;
        lobbyCode.text = "Lobby Code: "+lobby.LobbyCode;

    }
}
