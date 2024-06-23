using System;
using System.Collections;
using System.Collections.Generic;
using ScriotableObjects;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameMultiplayer : NetworkBehaviour
{
    public static KitchenGameMultiplayer Instance { get; private set; }

    [SerializeField] private KitchenObjectListSO kitchenObjectListSo;
    private void Awake()
    {
        Instance = this;
    }
    public  void SpawnKitchenObject(KitchenObjectSo kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
    {
       SpawnKitchenObjectServerRpc(GetKitchenObjectSOIndex(kitchenObjectSO),kitchenObjectParent.GetNetworkObject());   
    }
    
    [ServerRpc(RequireOwnership =false)]
    private void SpawnKitchenObjectServerRpc(int kitchenObjectSOIndex, NetworkObjectReference kitchenObjectParentNetworkObjectReference)
    {
        KitchenObjectSo kitchenObjectSO = GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        Transform kitchenObjectTransform=Instantiate(kitchenObjectSO.prefab);

        NetworkObject kitchenObjectNetworkObject = kitchenObjectTransform.GetComponent<NetworkObject>();
        kitchenObjectNetworkObject.Spawn(true);
        
        KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();

        kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
        IKitchenObjectParent kitchenObjectParent =
            kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
        
        kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
    }

    private int GetKitchenObjectSOIndex(KitchenObjectSo kitchenObjectSo)
    {
        return kitchenObjectListSo.KitchenObjectSOList.IndexOf(kitchenObjectSo);
    }
    private KitchenObjectSo GetKitchenObjectSOFromIndex(int kitchenObjectSoIndex)
    {
        return kitchenObjectListSo.KitchenObjectSOList[kitchenObjectSoIndex];
    }
}
