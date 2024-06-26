using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using Unity.Netcode;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public enum State
    {
        Idle,
        Frying,
        Fried,
        Burnt,
    }
    [SerializeField] private FryingRecipeSO[] _fryingRecipeSoArray;
    [SerializeField] private BurningRecipeSO[] _burningRecipeSoArray;

    private NetworkVariable<State> state= new NetworkVariable<State>(State.Idle);
    private NetworkVariable<float> fryingTimer= new NetworkVariable<float>(0f);
    private NetworkVariable<float> burningTimer=new NetworkVariable<float>(0f);
    private FryingRecipeSO _fryingRecipeSo;
    private BurningRecipeSO _burningRecipeSo;
    

    public override void OnNetworkSpawn()
    {
        fryingTimer.OnValueChanged += OnValueChanged_FryingTimer;
        burningTimer.OnValueChanged += OnValueChanged_BurningTimer;
        state.Value =  State.Idle;
        state.OnValueChanged += OnValueChanged_CookingState;
    }

    private void OnValueChanged_CookingState(State previousvalue, State newvalue)
    {
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state =  state.Value
        });
        
        if (state.Value == State.Burnt || state.Value == State.Idle)
        {
            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
            {
                progressNormalized = 0f
            });
        }
    }

    private void OnValueChanged_BurningTimer(float previousvalue, float newvalue)
    {
        float burningTimerMaxLocal= _burningRecipeSo != null ? _burningRecipeSo.burningTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = burningTimer.Value/burningTimerMaxLocal
        });
    }

    private void OnValueChanged_FryingTimer(float previousvalue, float newvalue)
    {
        float fryingTimerMaxLocal= _fryingRecipeSo != null ? _fryingRecipeSo.fryingTimerMax : 1f;
        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
        {
            progressNormalized = fryingTimer.Value/fryingTimerMaxLocal
        });
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        if (HasKitchenObject())
        {
            switch (state.Value)
            {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer.Value += Time.deltaTime;
                        
                        
                        
                        
                        if (fryingTimer.Value > _fryingRecipeSo.fryingTimerMax)
                        {
                            //fried

                            KitchenObject.DestroyKitchenObject(GetKitchenObject());

                            KitchenObject.SpawnKitchenObject(_fryingRecipeSo.output, this);
                            
                            state.Value = State.Fried;
                            burningTimer.Value = 0f;
                            SetBurningRecipeSOClientRpc(
                                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(GetKitchenObject().GetKitchenObjectSO()));


                        }

                        break;
                    case State.Fried:
                        burningTimer.Value += Time.deltaTime;
                       /// Debug.Log(burningTimer);
                       // Debug.Log(_burningRecipeSo);
                       
                        
                        if (burningTimer.Value > _burningRecipeSo.burningTimerMax)
                        {
                            //fried
                            burningTimer.Value = 0f;
                            KitchenObject.DestroyKitchenObject(GetKitchenObject());
                            

                            KitchenObject.SpawnKitchenObject(_burningRecipeSo.output, this);
                            state.Value = State.Burnt;
                            
                           
                        }
                        break;
                    case State.Burnt:
                        break;
            }
        }
        
    }

    public override void Interact(Player player)
    {
        
        if(!HasKitchenObject())
        {
            //there is no kitchenobject here
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {//player carrying something that can be fried
                    KitchenObject kitchenObject = player.GetKitchenObject();
                    kitchenObject.SetKitchenObjectParent(this);
                    int index = KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObject.GetKitchenObjectSO());
                    InteractLogicOnPlacedObjectServerRpc(index);
                    
                    
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
            
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlatekitchenObject platekitchenObject))
                {
                    // player is holding a plate
                    if (platekitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        KitchenObject.DestroyKitchenObject(GetKitchenObject());
                        SetStateIdleServerRpc();

                    }
                    
                }
           
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                SetStateIdleServerRpc();

            }
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetStateIdleServerRpc()
    {
        state.Value = State.Idle;
    }
    [ServerRpc(RequireOwnership = false)]
    private void InteractLogicOnPlacedObjectServerRpc(int recipeIndex)
    {
        SetFryingRecipeSOClientRpc(recipeIndex);
        fryingTimer.Value = 0f;
        state.Value = State.Frying;
    }
    [ClientRpc]
    private void SetFryingRecipeSOClientRpc(int recipeIndex)
    {
        _fryingRecipeSo = GetFryingRecipeSOWithInput(KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(recipeIndex));
    }
    [ClientRpc]
    private void SetBurningRecipeSOClientRpc(int recipeIndex)
    {
        _burningRecipeSo = GetBurningRecipeSOWithInput(KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(recipeIndex));
    }
    private bool HasRecipeWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        return fryingRecipeSo != null;
    }
    
    private KitchenObjectSo GetOutputForInput(KitchenObjectSo inputKitchenObjectSo)
    {
        FryingRecipeSO fryingRecipeSo = GetFryingRecipeSOWithInput(inputKitchenObjectSo);
        if (fryingRecipeSo != null)
        {
            return fryingRecipeSo.output;
        }

        return null;

    }

    private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        foreach (FryingRecipeSO fryingRecipeSo in _fryingRecipeSoArray)
        {
            if (inputKitchenObjectSo == fryingRecipeSo.input)
            {
                return fryingRecipeSo;
            }
        }

        return null;
    }
    private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSo inputKitchenObjectSo)
    {
        foreach (BurningRecipeSO burningRecipeSo in _burningRecipeSoArray)
        {
            if (inputKitchenObjectSo == burningRecipeSo.input)
            {
                return burningRecipeSo;
            }
        }

        return null;
    }

    public bool IsFried()
    {
        return state.Value == State.Fried;
    }
}
