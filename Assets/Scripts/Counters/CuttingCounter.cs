using System;
using Unity.Netcode;
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
                Debug.Log("no object ");
                Debug.Log(player.HasKitchenObject());
                if (player.HasKitchenObject())
                {
                    //player is carrying something
                    Debug.Log(HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()));
                    if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                    {//player carrying something that can be cut
                        KitchenObject kitchenObject = player.GetKitchenObject();
                        Debug.Log("there is"+ kitchenObject.name) ;
                        kitchenObject.SetKitchenObjectParent(this);
                        
                        InteractLogicPlaceObjectOnCounterServerRpc();
                    }
                
                }
                else
                {
                    //player not carrying anything
                    Debug.Log("no object in hand");
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
                        if (platekitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                        {
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        }
                    
                    }
              
                }
                else
                {
                    GetKitchenObject().SetKitchenObjectParent(player);
                }
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void InteractLogicPlaceObjectOnCounterServerRpc()
        {
            InteractLogicPlaceObjectOnCounterClientRpc();
        }
        [ClientRpc]
        private void InteractLogicPlaceObjectOnCounterClientRpc()
        {
            cuttingProgress = 0;
            
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }

        public override void InteractAlternate(Player player)
        {
            if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO()))
            {
                //there is a kitchen object
               CutObjectServerRpc();
               TestCuttingProgressDoneServerRpc();
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void CutObjectServerRpc()
        {
            CutObjectClientRpc();
        }
        [ClientRpc]
        private void CutObjectClientRpc()
        {
            cuttingProgress++;
            OnCut?.Invoke(this,EventArgs.Empty);
            OnAnyCut?.Invoke(this,EventArgs.Empty);
            
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
            });
            
            
        }
        [ServerRpc(RequireOwnership = false)]
        private void TestCuttingProgressDoneServerRpc()
        {
            CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
            if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
            {
                KitchenObjectSo outputKitchenObjectSo = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
                
                KitchenObject.DestroyKitchenObject(GetKitchenObject());
                
                KitchenObject.SpawnKitchenObject(outputKitchenObjectSo, this);
                _isCutComplete = true;
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
