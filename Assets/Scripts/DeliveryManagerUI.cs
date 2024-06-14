using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour
{
    
    
    [SerializeField] private Transform container;
    [SerializeField] private Transform recipeTemplate;

    private void Awake()
    {
        recipeTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeAdded += OnWaitingRecipeListChange;
        DeliveryManager.Instance.OnRecipeCompleted += OnWaitingRecipeListChange;
    }

    private void OnWaitingRecipeListChange(object sender, EventArgs e)
    {
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        foreach (Transform child in container)
        {
            if (child != recipeTemplate)
            {
                Destroy(child.gameObject);
            }

            foreach (var waitingOrder in DeliveryManager.Instance.GetWaitingRecipesList())
            {
                Transform instantiatedTemplate = Instantiate(recipeTemplate, container);
                instantiatedTemplate.gameObject.SetActive(true);
            }
        }
    }
}
