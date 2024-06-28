using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectReady :NetworkBehaviour
{
    public static CharacterSelectReady Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;
    [SerializeField] public TextMeshProUGUI buttonText;
    
    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        
    }

    public void SetPlayerReady()
    {
        Debug.Log("playerReady set");
        SetPlayerReadyServerRpc();
        Debug.Log("playerReady set");
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        buttonText.text = "ServerRpc";
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
    
        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds) {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId]) {
                // This player is NOT ready
                Debug.Log("playerReady");
                allClientsReady = false;
                break;
            }
        }
        

        if (allClientsReady) {
            Debug.Log("all clients ready");
            Loader.LoadNetwork(Loader.Scene.GameScene);
        }
    }
}
