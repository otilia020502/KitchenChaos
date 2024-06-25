using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : NetworkBehaviour
{
   [SerializeField] private KitchenObjectSo kitchenObjectSo;
   
   
   private IKitchenObjectParent kitchenObjectParent;
   private FollowTransform _followTransform;


   protected virtual void Awake()
   {
      _followTransform = GetComponent<FollowTransform>();
   }

   public KitchenObjectSo GetKitchenObjectSo()
   {
      return kitchenObjectSo;
   }

   public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
   {
     SetKitchenObjectParentServerRpc(kitchenObjectParent.GetNetworkObject());
      
   }

   [ServerRpc(RequireOwnership = false)]
   private void SetKitchenObjectParentServerRpc(NetworkObjectReference kitchenObjectParentnetworkObjectReference)
   {
      SetKitchenObjectParentClientRpc(kitchenObjectParentnetworkObjectReference);
   }

   [ClientRpc]
   private void SetKitchenObjectParentClientRpc(NetworkObjectReference kitchenObjectParentNetworkObjectReference)
   {
      kitchenObjectParentNetworkObjectReference.TryGet(out NetworkObject kitchenObjectParentNetworkObject);
      IKitchenObjectParent kitchenObjectParent = kitchenObjectParentNetworkObject.GetComponent<IKitchenObjectParent>();
      
      if (this.kitchenObjectParent != null)
      {
         this.kitchenObjectParent.ClearKitchenObject();
      }
      
      
      this.kitchenObjectParent = kitchenObjectParent;
      if (kitchenObjectParent.HasKitchenObject())
      {
         Debug.LogError("Counter already has object"+ kitchenObjectParent.GetKitchenObject().name);
         return;
      }
      kitchenObjectParent.SetKitchenObject(this);
      
      _followTransform.SetTargetTransform(kitchenObjectParent.GetKitchenObjectFollowTransform());
   }
   public IKitchenObjectParent GetKitchenObjectParent()
   {
      return kitchenObjectParent;
   }

   public void DestorySelf()
   {
      
      Destroy(gameObject);
   }

   public void ClearKitchenObjectOnParent()
   {
      kitchenObjectParent.ClearKitchenObject();
   }

   public bool TryGetPlate(out PlatekitchenObject platekitchenObject)
   {
      if (this is PlatekitchenObject)
      {
         platekitchenObject= this as PlatekitchenObject;
         return true;
      }
      else
      {
         platekitchenObject = null;
         return false;
      }
   }

   public static void SpawnKitchenObject(KitchenObjectSo kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
   {
      KitchenGameMultiplayer.Instance.SpawnKitchenObject(kitchenObjectSO, kitchenObjectParent);
      
   }
   public static void DestroyKitchenObject(KitchenObject kitchenObject)
   {
      KitchenGameMultiplayer.Instance.DestroyKitchenObject(kitchenObject);
      
   }
}
