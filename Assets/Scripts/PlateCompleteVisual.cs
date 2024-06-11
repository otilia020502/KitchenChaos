using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour
{

    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSo kitchenoObjectSo;
        public GameObject gameObject;
    }
    [SerializeField] private PlatekitchenObject _platekitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _kitchenObjectSoGameObjects;
    private void Start()
    {
        _platekitchenObject.OnIngredientAdded += PlatekitchenObject_OnIngredientAdded;
        
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in _kitchenObjectSoGameObjects)
        {
            kitchenObjectSoGameObject.gameObject.SetActive(false);
        }
    }

    private void PlatekitchenObject_OnIngredientAdded(object sender, PlatekitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in _kitchenObjectSoGameObjects)
        {
            if (kitchenObjectSoGameObject.kitchenoObjectSo == e.KitchenObjectSo)
            {
                kitchenObjectSoGameObject.gameObject.SetActive(true);
            }
        }
    }
}
