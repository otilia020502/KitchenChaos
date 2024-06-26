using System;
using UnityEngine;

namespace Counters
{
    public class CuttingCounter : BaseCounter, IHasProgress
    {
        public static event EventHandler OnAnyCut ;

        new public static void ResetStaticData()
        {
            OnAnyCut = null;
        }
        public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
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
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
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
                    if (player.GetKitchenObject().TryGetPlate(out PlatekitchenObject platekitchenObject))
                    {
                        // player is holding a plate
                        if (platekitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                        {
                            GetKitchenObject().DestorySelf();
                        }
                    
                    }
              
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
                OnAnyCut?.Invoke(this,EventArgs.Empty);
            
                CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
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
}
