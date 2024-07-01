using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using Random = UnityEngine.Random;

public class KitchenGameLobby : MonoBehaviour
{
   
   public static KitchenGameLobby Instance { get; private set; }

   private Lobby joinedLobby;
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

   public async void CreateLobby(string lobbyName, bool isPrivate)
   {
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
      }
   }

   public async void QuickJoin()
   {
      try
      {
         joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
         
         KitchenGameMultiplayer.Instance.StartClient();
      }
      catch (LobbyServiceException e)
      {
         Debug.LogError(e);
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
      }
   }

   public Lobby GetLobby()
   {
      return joinedLobby;
   }


}
