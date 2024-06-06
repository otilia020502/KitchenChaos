using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ClearCounter : BaseCounter, IKitchenObjectParent
{
    [SerializeField] private Transform counterTopPoint;
    [SerializeField] private KitchenObjectSo  kitchenObjectSO;
  
    
    private KitchenObject kitchenObject;

 
    public override void Interact(Player player)
    {
        if (kitchenObject == null)
        {
            Transform kitchenObjectTransform=Instantiate(kitchenObjectSO.prefab, counterTopPoint);
            kitchenObjectTransform.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
            
        }
        else
        {
            kitchenObject.SetKitchenObjectParent(player);
           
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return counterTopPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
   
}
