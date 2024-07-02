using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
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
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;
    private void Awake()
    {
        mainMenuButton.onClick.AddListener((() =>
        {
            KitchenGameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MyMenuScene);
            
        }));
        quickJoinButton.onClick.AddListener((() => KitchenGameLobby.Instance.QuickJoin()));
        createLobbyButton.onClick.AddListener((() => _lobbyCreateUI.Show()));
        joinCodeButton.onClick.AddListener((() => KitchenGameLobby.Instance.JoinWithCode(joinCodeInput.text)));
    }

    private void Start()
    {
        KitchenGameLobby.Instance.OnLobbyListChanged += KitchenGameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
        
        lobbyTemplate.gameObject.SetActive(false);
    }

    private void KitchenGameLobby_OnLobbyListChanged(object sender, KitchenGameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> list)
    {
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            
            Destroy(child);
            
        }

        foreach (var lobby in list)
        {
           Transform newLobby=  Instantiate(lobbyTemplate, lobbyContainer);
           newLobby.gameObject.SetActive(true);
           newLobby.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }
}
