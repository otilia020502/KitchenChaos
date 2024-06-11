using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatekitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSo KitchenObjectSo;
    }

    [SerializeField] private List<KitchenObjectSo> validKit;
    private List<KitchenObjectSo> _kitchenObjectSoList= new List<KitchenObjectSo>();
    public bool TryAddIngredient(KitchenObjectSo kitchenObjectSo)
    {
        if (!validKit.Contains(kitchenObjectSo))
        {
            return false;
        }
        if (_kitchenObjectSoList.Contains(kitchenObjectSo))
        {
            //already in list
            return false;
        }
        else
        {
            _kitchenObjectSoList.Add(kitchenObjectSo);
            
            OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
            {
                KitchenObjectSo = kitchenObjectSo
            });
            return true;
        }
        
    }
}
