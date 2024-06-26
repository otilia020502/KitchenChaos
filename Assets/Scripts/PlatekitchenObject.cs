using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlatekitchenObject : KitchenObject
{

    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSo KitchenObjectSo;
    }

    [SerializeField] private List<KitchenObjectSo> validKit;
    
    
    private List<KitchenObjectSo> _kitchenObjectSoList;

    protected override void Awake()
    {
        base.Awake();
        _kitchenObjectSoList= new List<KitchenObjectSo>();
    }

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
            AddIngredientServerRpc(
                KitchenGameMultiplayer.Instance.GetKitchenObjectSOIndex(kitchenObjectSo)
                );
            
            return true;
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    private void AddIngredientServerRpc(int kitchenObjectSOIndex)
    {
        AddIngredientClientRpc(kitchenObjectSOIndex);
    }
    [ClientRpc]
    private void AddIngredientClientRpc(int kitchenObjectSOIndex)
    {
        KitchenObjectSo kitchenObjectSo =
            KitchenGameMultiplayer.Instance.GetKitchenObjectSOFromIndex(kitchenObjectSOIndex);
        _kitchenObjectSoList.Add(kitchenObjectSo);

        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            KitchenObjectSo = kitchenObjectSo
        });
    }

    public List<KitchenObjectSo> GetIngridientSos()
    {
        return _kitchenObjectSoList;
    }
}
