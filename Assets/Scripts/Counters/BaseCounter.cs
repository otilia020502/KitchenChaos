using System;
using Unity.Netcode;
using UnityEngine;

namespace Counters
{
    public class BaseCounter : NetworkBehaviour, IKitchenObjectParent
    {
        public static event EventHandler OnAnyObjectPlacedHere;
        [SerializeField] private Transform counterTopPoint;
        public static void ResetStaticData()
        {
            OnAnyObjectPlacedHere = null;
        }
    
        private KitchenObject kitchenObject;
        public virtual void Interact(Player player)
        {
            Debug.LogError("BaseCounter.Interact()");
        }
        public virtual void InteractAlternate(Player player)
        {
            Debug.LogError("BaseCounter.InteractAlternate()");
        }
        public Transform GetKitchenObjectFollowTransform()
        {
            return counterTopPoint;
        }

        public void SetKitchenObject(KitchenObject kitchenObject)
        {
            this.kitchenObject = kitchenObject;
            if (kitchenObject != null)
            {
                OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
            }
        }

        public KitchenObject GetKitchenObject()
        {
            return kitchenObject;
        }

        public void ClearKitchenObject()
        {
            kitchenObject = null;
        }

        public bool HasKitchenObject()
        {
            return kitchenObject != null;
        }
    
        public NetworkObject GetNetworkObject()
        {
            return NetworkObject;
        }
    
    }
}
