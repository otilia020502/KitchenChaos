using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{
    public static KitchenGameManager Instance { get; private set; }
    public event EventHandler OnStateChanged;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnPaused;
    public event EventHandler OnPlayerReady;
    
    
    
    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver,
    }

    public Dictionary<ulong, bool> playersReadyDictionary;
    private NetworkVariable<State> state= new NetworkVariable<State>(State.WaitingToStart);
    private float waitingToStartTimer = 3f;
    private float countDownToStartTimer = 3f;
    private float gamePlayingTimer;
    private float gamePlayingTimerMax=300f;
    private bool isGamePaused = false;
    public bool isLocalPlayerReady=false;
    private void Awake()
    {
        Instance = this;
        state.Value = State.WaitingToStart;
        playersReadyDictionary= new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnInteractAction += GameManager_OnPlayerInteract;
        //debug trigger game start automatically
        

    }

    private void GameManager_OnPlayerInteract(object sender, EventArgs e)
    {
        Debug.Log(state.Value);
        if (state.Value==State.WaitingToStart)
        {
            isLocalPlayerReady = true;
            SetPlayerReadyServerRpc();
            OnPlayerReady?.Invoke(this, EventArgs.Empty);
            bool allPlayersReady = true;
            foreach (var client in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (playersReadyDictionary.ContainsKey(client) )
                {
                    if (playersReadyDictionary.TryGetValue(client, out bool isClientReady))
                    {
                        if (isClientReady == false)
                        {
                            allPlayersReady = false;
                        }
                    }
                    else
                    {
                        allPlayersReady = false;
                    }
                }
                else
                {
                    allPlayersReady = false;
                }
            }

            if (allPlayersReady)
            {
                state.Value = State.CountDownToStart;
                
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        state.Value = State.WaitingToStart;
        state.OnValueChanged += KitchenManager_OnStateChanged;
    }

    private void KitchenManager_OnStateChanged(State previousvalue, State newvalue)
    {
        Debug.Log("State was changed "+ state.Value);
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
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
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer <= 0f)
                {
                    
                }
                
                break;
            case State.CountDownToStart:
                countDownToStartTimer -= Time.deltaTime;
                if (countDownToStartTimer <= 0f)
                {
                    state.Value = State.GamePlaying;
                    
                    gamePlayingTimer = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer <= 0f)
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

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams= default)
    {
        playersReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;
    }
    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
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

    public float GetPlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer / gamePlayingTimerMax);
    }

    public void TogglePauseGame()
    {
        isGamePaused = !isGamePaused;
        if (isGamePaused)
        {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnGameUnPaused?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
