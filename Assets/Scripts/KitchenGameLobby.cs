using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class KitchenGameLobby : MonoBehaviour
{
   
   public static KitchenGameLobby Instance { get; private set; }

   public event EventHandler OnCreateLobbyStarted;
   public event EventHandler OnCreateLobbyFailed;
   public event EventHandler OnJoinStarted;
   public event EventHandler OnQuickJoinFailed;
   public event EventHandler OnJoinFailed;

   public event EventHandler<OnLobbyListChangedEventArgs> OnLobbyListChanged;
   public class OnLobbyListChangedEventArgs: EventArgs
   {
      public List<Lobby> lobbyList;
   }
   private Lobby joinedLobby;
   private float heartBeatTimer;
   private float lobbyChangeTimer;
   private void Awake()
   {
      Instance = this;
      DontDestroyOnLoad(gameObject);
      InitializeUnityAuthentication();
   }

   private async void InitializeUnityAuthentication()
   {
      if (UnityServices.State != ServicesInitializationState.Initialized)
      {
         InitializationOptions initializationOptions = new InitializationOptions();
         initializationOptions.SetProfile(UnityEngine.Random.Range(0, 100000).ToString());
         await UnityServices.InitializeAsync(initializationOptions);

         await AuthenticationService.Instance.SignInAnonymouslyAsync();
      }
     
   }

   private void Update()
   {
      HeartBeat();
      HandleLobbyListChanged();
   }

   private void HandleLobbyListChanged()
   {
      if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn)
      {
         lobbyChangeTimer -= Time.deltaTime;
         if (lobbyChangeTimer < 0)
         {
            float lobbyChangeTimeMax = 3f;
            lobbyChangeTimer = lobbyChangeTimeMax;
           ListLobbies();
         }
      }
     
   }
   private void HeartBeat()
   {
      if (IsLobbyHost())
      {
         heartBeatTimer -= Time.deltaTime;
         if (heartBeatTimer <= 0f)
         {
            float heartbeatTimerMax = 15f;
            heartBeatTimer = heartbeatTimerMax;

            LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
         }
      }
   }

   private bool IsLobbyHost()
   {
      return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
   }

   private async void ListLobbies()
   {
      try
      {
         QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
         {
            Filters = new List<QueryFilter>
            {
               new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
            }
         };
         QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
         OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
         {
            lobbyList = queryResponse.Results
         });
      }
      catch (LobbyServiceException e)
      {
         Debug.LogError(e);
      }
   }

   public async void CreateLobby(string lobbyName, bool isPrivate)
   {
      OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
      try
      {
         joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, KitchenGameMultiplayer.MAX_PLAYER_COUNT,
            new CreateLobbyOptions
            {
               IsPrivate = isPrivate,
            });
         KitchenGameMultiplayer.Instance.StartHost();
         Loader.LoadNetwork(Loader.Scene.MyCharacterSelectScene);
      }
      catch (LobbyServiceException e)
      {
         Debug.LogError(e);
         OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
      }
   }

   public async void QuickJoin()
   {
      OnJoinStarted?.Invoke(this, EventArgs.Empty);
      try
      {
         joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
         
         KitchenGameMultiplayer.Instance.StartClient();
      }
      catch (LobbyServiceException e)
      {
         Debug.LogError(e);
         OnQuickJoinFailed?.Invoke(this, EventArgs.Empty);
      }
      
   }

   public async void JoinWithCode(string code)
   {
      try
      {
         joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(code);
         KitchenGameMultiplayer.Instance.StartClient();
      }
      catch(LobbyServiceException e)
      {
         Debug.LogError(e);
         OnJoinFailed?.Invoke(this, EventArgs.Empty);
      }
   }
   public async void JoinWithId(string code)
   {
      try
      {
         joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(code);
         KitchenGameMultiplayer.Instance.StartClient();
      }
      catch(LobbyServiceException e)
      {
         Debug.LogError(e);
         OnJoinFailed?.Invoke(this, EventArgs.Empty);
      }
   }

   public async void DeleteLobby()
   {
      if (joinedLobby != null)
      {
         try
         {
            LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
            joinedLobby = null;
         }
         catch (LobbyServiceException e)
         {
            Debug.LogError(e);
         }
      }
   }
   public Lobby GetLobby()
   {
      return joinedLobby;
   }

   public  async void LeaveLobby()
   {
      if (joinedLobby != null)
      {
         try
         {
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);
            joinedLobby = null;
         }
         catch(LobbyServiceException e)
         {
            Debug.LogError(e);
         }
      }
     
   }


}
