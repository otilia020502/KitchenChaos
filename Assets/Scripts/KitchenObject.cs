using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
   [SerializeField] private KitchenObjectSo kitchenObjectSo;
   
   private IKitchenObjectParent kitchenObjectParent;

   public KitchenObjectSo GetKitchenObjectSo()
   {
      return kitchenObjectSo;
   }

   public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
   {
      if (this.kitchenObjectParent != null)
      {
         this.kitchenObjectParent.ClearKitchenObject();
      }
      
      this.kitchenObjectParent = kitchenObjectParent;
      if (kitchenObjectParent.HasKitchenObject())
      {
         Debug.LogError("Counter already has object");
      }
      kitchenObjectParent.SetKitchenObject(this);
      
      transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
      transform.localPosition = Vector3.zero;
   }

   public IKitchenObjectParent GetKitchenObjectParent()
   {
      return kitchenObjectParent;
   }

   public void DestorySelf()
   {
      kitchenObjectParent.ClearKitchenObject();
      Destroy(gameObject);
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

   public static KitchenObject SpawnKitchenObject(KitchenObjectSo kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
   {
      Transform kitchenObjectTransform=Instantiate(kitchenObjectSO.prefab);
      KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
      kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
      return kitchenObject;
   }
}
