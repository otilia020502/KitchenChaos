using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    [SerializeField] private KitchenObjectSo  kitchenObjectSO;
  
    

 
    public override void Interact(Player player)
    {
        if (!HasKitchenObject())
        {
            //there is no kitchenobject here
            if (player.HasKitchenObject())
            {
                //player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
            }
            else
            {
                //player not carrying anything
            }
        }
        else
        {
            //there is a kitchenobject here
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlatekitchenObject platekitchenObject))
                {
                    // player is holding a plate
                    if (platekitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestorySelf();
                    }
                    
                }
                else
                {//player not carrying plate but something else
                    if (GetKitchenObject().TryGetPlate(out platekitchenObject))
                    {
                        //counter holding a plate
                        if (platekitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSo()))
                        {
                            player.GetKitchenObject().DestorySelf();
                            
                        }
                    }
                    
                }
                
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

   
   
}
