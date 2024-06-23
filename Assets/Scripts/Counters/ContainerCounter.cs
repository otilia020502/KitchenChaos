using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using Unity.Netcode;
using UnityEngine;

public class ContainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    
    [SerializeField] private KitchenObjectSo  kitchenObjectSO;

    
    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //player is not carrying anything
            KitchenObject.SpawnKitchenObject(kitchenObjectSO, player);
        
           InteractLogicServerRpc();
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
       OnPlayerGrabbedObject?.Invoke(this,EventArgs.Empty);
    }
}
