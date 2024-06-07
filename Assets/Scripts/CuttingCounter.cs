using System.Collections;
using System;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
    public event EventHandler<OnProgressChangedEventArgs> OnProgressChanged;

    public class OnProgressChangedEventArgs : EventArgs
    {
        public float progressNormalized;
    }

    public event EventHandler OnCut ;
    [SerializeField] private CuttingRecipeSO []arrayCuttingRecipeSO;

    private int cuttingProgress;
    private bool _isCutComplete=true;
    public override void Interact(Player player)
    {
        if(!HasKitchenObject())
        {
            //there is no kitchenobject here
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                {//player carrying something that can be cut
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    cuttingProgress = 0;
                    //_isCutComplete = false;
                    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                    OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
                    {
                        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
                    });
                }
                
            }
            else
            {
                //player not carrying anything
            }
        }
        else
        {
            //there is a kitchenobject here
            if (!_isCutComplete)
            {
                // Object is not fully cut yet, can't pick it up
                return;
            }
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
            OnCut?.Invoke(this,EventArgs.Empty);
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
            OnProgressChanged?.Invoke(this, new OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSo outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSo());
                GetKitchenObject().DestorySelf();
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
                _isCutComplete = true;
            }
        }
    }

    private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSo);
        return cuttingRecipeSO != null;
    }
    
    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
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
