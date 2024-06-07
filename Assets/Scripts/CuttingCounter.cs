using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    
    [SerializeField] private CuttingRecipeSO []arrayCuttingRecipeSO;

    private int cuttingProgress;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            //there is no kitchenobject here
            if (player.HasKitchenObject())
            {
                //player is carrying something
                player.GetKitchenObject().SetKitchenObjectParent(this);
                cuttingProgress = 0;
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
        if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSo()))
        {
            //there is a kitchen object
            cuttingProgress++;
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
            KitchenObjectSo outputKitchenObjectSo = GerOutputForInput(GetKitchenObject().GetKitchenObjectSo());
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                GetKitchenObject().DestorySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);
        return cuttingRecipeSO != null;
    }
    
    private KitchenObjectSo GerOutputForInput(KitchenObjectSo inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);
        if (cuttingRecipeSO != null)
        {
            return cuttingRecipeSO.output;
        }

        return null;

    }

    private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        foreach (CuttingRecipeSO cuttingRecipeSo in arrayCuttingRecipeSO)
        {
            if (inputKitchenObjectSo == cuttingRecipeSo.input)
            {
                return cuttingRecipeSo;
            }
        }

        return null;
    }
}
