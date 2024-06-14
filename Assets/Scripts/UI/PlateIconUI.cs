using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlateIconUI : MonoBehaviour
{
    [SerializeField] private PlatekitchenObject _platekitchenObject;
    [SerializeField] private Transform iconTemplatePrefab;
    [SerializeField] private Image iconIngredient;
    private GameObject[] _spawnedTemplatesArray;
    private List<GameObject> _spawnedTemplatesList= new List<GameObject>();
    private void Start()
    {
        _platekitchenObject.OnIngredientAdded += OnPlateIngredientAdded;
    }

    private void OnPlateIngredientAdded(object sender, PlatekitchenObject.OnIngredientAddedEventArgs e)
    {
        //int index = 0;
        _spawnedTemplatesArray = new GameObject[_spawnedTemplatesList.Count];
        Debug.Log(_platekitchenObject.GetIngridientSos().Count);
        for (int i = 0; i<_spawnedTemplatesArray.Length; i++)
        {
            _spawnedTemplatesArray[i] = _spawnedTemplatesList.ElementAt(i);
            
        }
        for (int i = 0; i<_spawnedTemplatesArray.Length; i++)
        {
            Destroy(_spawnedTemplatesArray[i]);
            
        }

        
        _spawnedTemplatesList.Clear();
       
        foreach (KitchenObjectSo kitchenObjectSo in _platekitchenObject.GetIngridientSos())
        {
            iconIngredient.sprite = kitchenObjectSo.sprite;
            var instantiated = Instantiate(iconTemplatePrefab, transform);
            _spawnedTemplatesList.Add(instantiated.gameObject);
        }
    }
}
