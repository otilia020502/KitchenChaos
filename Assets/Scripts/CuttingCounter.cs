using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    //[SerializeField] private KitchenObjectSo cutKitchenObjectSO;
    [SerializeField] private CuttingRecipeSO []arrayCuttingRecipeSO;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
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
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
            }
        }
    }

    public override void InteractAlternate(Player player)
    {
        if (HasKitchenObject())
        {
            //there is a kitchen object
            KitchenObjectSo outputKitchenObjectSo = GerOutputForInput(GetKitchenObject().GetKitchenObjectSo());
            GetKitchenObject().DestorySelf();
            KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
        }
    }
    
    private KitchenObjectSo GerOutputForInput(KitchenObjectSo inputKitchenObjectSo)
    {
        foreach (CuttingRecipeSO cuttingRecipeSo in arrayCuttingRecipeSO)
        {
            if (inputKitchenObjectSo == cuttingRecipeSo.input)
            {
                return cuttingRecipeSo.output;
            }
        }

        return null;
    }
}
