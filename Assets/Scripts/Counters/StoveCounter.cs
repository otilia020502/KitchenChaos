using System;
using System.Collections;
using System.Collections.Generic;
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

    private State state;
    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO _fryingRecipeSo;
    private BurningRecipeSO _burningRecipeSo;
    private void Start()
    {
        state =  State.Idle;
    }

    private void Update()
    {
        if (HasKitchenObject())
        {
            switch (state)
            {
                    case State.Idle:
                        break;
                    case State.Frying:
                        fryingTimer += Time.deltaTime;
                        
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = fryingTimer/_fryingRecipeSo.fryingTimerMax
                        });
                        
                        
                        if (fryingTimer > _fryingRecipeSo.fryingTimerMax)
                        {
                            //fried

                            GetKitchenObject().DestorySelf();

                            KitchenObject.SpawnKitchenObject(_fryingRecipeSo.output, this);
                            
                            state = State.Fried;
                            burningTimer = 0f;
                            _burningRecipeSo = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state =  state
                            });
                        }

                        break;
                    case State.Fried:
                        burningTimer += Time.deltaTime;
                        Debug.Log(burningTimer);
                        Debug.Log(_burningRecipeSo);
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = burningTimer/_burningRecipeSo.burningTimerMax
                        });
                        
                        if (burningTimer > _burningRecipeSo.burningTimerMax)
                        {
                            //fried
                            burningTimer = 0f;

                            GetKitchenObject().DestorySelf();

                            KitchenObject.SpawnKitchenObject(_burningRecipeSo.output, this);
                            state = State.Burnt;
                            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                            {
                                state =  state
                            });
                            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                            {
                                progressNormalized = 0f
                            });
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
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSo()))
                {//player carrying something that can be fried
                    player.GetKitchenObject().SetKitchenObjectParent(this);
                    _fryingRecipeSo = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSo());
                    state = State.Frying;
                    fryingTimer = 0f;
                    OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                    {
                        state =  state
                    });
                    OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                    {
                       progressNormalized = fryingTimer/_fryingRecipeSo.fryingTimerMax
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
            
            if (player.HasKitchenObject())
            {
                //player is carrying something
                if (player.GetKitchenObject().TryGetPlate(out PlatekitchenObject platekitchenObject))
                {
                    // player is holding a plate
                    if (platekitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSo()))
                    {
                        GetKitchenObject().DestorySelf();
                        state = State.Idle;
                        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                        {
                            state =  state
                        });
                        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                        {
                            progressNormalized = 0f
                        });
                    }
                    
                }
           
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                state = State.Idle;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
                {
                    state =  state
                });
                OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs
                {
                    progressNormalized = 0f
                });
            }
        }
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
        return state == State.Fried;
    }
}
