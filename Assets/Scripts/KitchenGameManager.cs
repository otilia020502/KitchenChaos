using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalGamePaused;
    public event EventHandler OnLocalGameUnPaused;
    public event EventHandler OnPlayerReady;
    public event EventHandler OnMultiPlayerGamePaused;
    public event EventHandler OnMultiPlayerGameUnPaused;
    
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    
    private NetworkVariable<State> state= new NetworkVariable<State>(State.WaitingToStart);
    //private float waitingToStartTimer = 1f;
    private NetworkVariable<float> countDownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer= new NetworkVariable<float>(0f);
    private float gamePlayingTimerMax=90f;
    private bool isLocalGamePaused = false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    public bool isLocalPlayerReady=false;
    private bool autoTestGamePausedState;
    private Dictionary<ulong, bool> playersReadyDictionary;
    private Dictionary<ulong, bool> playerPauseDictionary;
    private void Awake()
    {
        Instance = this;
        playersReadyDictionary= new Dictionary<ulong, bool>();
        playerPauseDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameManager_OnPlayerInteract;
    }

    

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        state.OnValueChanged += KitchenManager_OnStateChanged;
        isGamePaused.OnValueChanged +=IsGamePaused_OnValueChanged;
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        autoTestGamePausedState = true;
        
    }

    private void IsGamePaused_OnValueChanged(bool previousvalue, bool newvalue)
    {
        if (isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiPlayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnMultiPlayerGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void KitchenManager_OnStateChanged(State previousvalue, State newvalue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    private void GameManager_OnPlayerInteract(object sender, EventArgs e)
    {
        Debug.Log(state.Value);
        if (state.Value==State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            OnPlayerReady?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
            Debug.Log("localplayer"+isLocalPlayerReady);
            
            
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams= default)
    {
        playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
        
        bool allPlayersReady = true;
        foreach (var client in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playersReadyDictionary.ContainsKey(client) || !playersReadyDictionary[client] )
            {
                allPlayersReady = false;
                break;

            }
            
        }
        Debug.Log("all players ready"+allPlayersReady);
        if (allPlayersReady)
        {
            state.Value = State.CountDownToStart;
                
        }
    }
    

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        switch (state.Value)
        {
            case State.WaitingToStart:
                break;
            case State.CountDownToStart:
                countDownToStartTimer.Value -= Time.deltaTime;
                if (countDownToStartTimer.Value <= 0f)
                {
                    state.Value = State.GamePlaying;
                    
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value <= 0f)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        //print(state);
        
    }

    private void LateUpdate()
    {
        if (autoTestGamePausedState == true)
        {
            autoTestGamePausedState = false;
            TestGamePausedState();
        }
    }
    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public bool GameIsPlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountDownStartActive()
    {
        return state.Value == State.CountDownToStart;
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer.Value / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isLocalGamePaused = !isLocalGamePaused;
        if (isLocalGamePaused)
        {
            PauseGameServerRpc();
            
            OnLocalGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnPauseGameServerRpc();
          
            OnLocalGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
        
    }
    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams=default)
    {
        playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
    }
    [ServerRpc(RequireOwnership = false)]
    private void UnPauseGameServerRpc(ServerRpcParams serverRpcParams=default)
    {
        playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = false;
        
        TestGamePausedState();
    }

    private void TestGamePausedState()
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (playerPauseDictionary.ContainsKey(clientId) && playerPauseDictionary[clientId])
            {
                //this player is paused
                isGamePaused.Value = true;
                return;
            }
        }
        //all players are unpaused
        isGamePaused.Value = false;
    }
    
}
