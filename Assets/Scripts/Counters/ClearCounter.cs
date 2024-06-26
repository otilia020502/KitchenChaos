
using Counters;
using UnityEngine;

public class ClearCounter : BaseCounter
{

    //[SerializeField] private KitchenObjectSo  kitchenObjectSO;
  
    

 
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
                    if (platekitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                    }
                    
                }
                else
                {//player not carrying plate but something else
                    if (GetKitchenObject().TryGetPlate(out platekitchenObject))
                    {
                        //counter holding a plate
                        if (platekitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                        {
                            player.GetKitchenObject().DestroySelf();
                            
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
