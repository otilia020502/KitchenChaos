using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame += KitchenGameMultiplayer_OnFailedToJoinGame;
        KitchenGameLobby.Instance.OnJoinStarted += KitchenGameLobby_OnJOinStarted;
        KitchenGameLobby.Instance.OnCreateLobbyStarted += KitchenGameLobby_OnCreateStarted;
        KitchenGameLobby.Instance.OnCreateLobbyFailed += KitchenGameLobby_OnCreateFailed;
        KitchenGameLobby.Instance.OnJoinFailed+= KitchenGameLobby_OnJoinFailed;
        KitchenGameLobby.Instance.OnQuickJoinFailed+= KitchenGameLobby_OnQuickJoinFailed;
        Hide();
    }

    private void KitchenGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
       ShowMessage("No lobbys found for quick join...");
    }

    private void KitchenGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Join failed...");
    }

    private void KitchenGameLobby_OnCreateFailed(object sender, EventArgs e)
    {
        ShowMessage("Creating lobby failed...");
    }

    private void KitchenGameLobby_OnCreateStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating lobby...");
    }

   

    private void KitchenGameLobby_OnJOinStarted(object sender, EventArgs e)
    {
       ShowMessage("trying to join...");
    }

    private void KitchenGameMultiplayer_OnFailedToJoinGame(object sender, EventArgs e)
    {
        
        messageText.text = NetworkManager.Singleton.DisconnectReason;
        if (messageText.text == "")
        {
           ShowMessage("Failed to connect");
        }
        else
        {
            ShowMessage( NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        KitchenGameMultiplayer.Instance.OnFailedToJoinGame -= KitchenGameMultiplayer_OnFailedToJoinGame;
    }
}
