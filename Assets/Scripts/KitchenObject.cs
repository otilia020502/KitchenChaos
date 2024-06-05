using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class KitchenObject : MonoBehaviour
{
   [SerializeField] private KitchenObjectSo kitchenObjectSo;
   
   private ClearCounter clearCounter;

   public KitchenObjectSo GetKitchenObjectSo()
   {
      return kitchenObjectSo;
   }

   public void SetClearCounter(ClearCounter clearCounter)
   {
      if (this.clearCounter != null)
      {
         this.clearCounter.ClearKitchenObject();
      }
      
      this.clearCounter = clearCounter;
      if (clearCounter.HasKitchenObject())
      {
         Debug.LogError("Counter already has object");
      }
      clearCounter.SetKitchenObject(this);
      
      transform.parent = clearCounter.GetKitchenObjectFollowTransform();
      transform.localPosition = Vector3.zero;
   }

   public ClearCounter GetClearCounter()
   {
      return clearCounter;
   }
}
