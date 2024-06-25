using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using Unity.Netcode;
using UnityEngine;

public class PlateCounter : BaseCounter
{
    public event EventHandler OnPlatesSpawned;
    public event EventHandler OnPlatesRemoved;
    
    private float spawnTime;
    private const float  maxSpawnTime=4f;
    private int platesAmount=4;
    private const int maxPlatesAmount = 5;
    
    [SerializeField] private KitchenObjectSo plateKitchenObjectSo;


    private void Start()
    {
        for (int i = 0; i < platesAmount; i++)
        {
            OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnTime += Time.deltaTime;

        if (spawnTime > maxSpawnTime)
        {
            spawnTime = 0f;
            if (KitchenGameManager.Instance.GameIsPlaying() && platesAmount < maxPlatesAmount)
            {
               SpawnPlateServerRpc();
            }
            
        }
    }
    [ServerRpc]
    private void SpawnPlateServerRpc()
    {
        SpawnPlateClientRpc();
    }
    [ClientRpc]
    private void SpawnPlateClientRpc()
    {
        platesAmount++;
        OnPlatesSpawned?.Invoke(this, EventArgs.Empty);
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            if (platesAmount > 0)
            {
                
                KitchenObject.SpawnKitchenObject(plateKitchenObjectSo, player);
                InteractLogicServerRpc();
            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicServerRpc()
    {
        InteractLogicClientRpc();
    }
    
    [ClientRpc]
    private void InteractLogicClientRpc()
    {
        platesAmount--;
        
        OnPlatesRemoved?.Invoke(this, EventArgs.Empty);
    }
}