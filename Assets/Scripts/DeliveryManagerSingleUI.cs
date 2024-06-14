using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipeNameText;
    [SerializeField] private Transform iconContainer;
    [FormerlySerializedAs("templateContainer")] [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSO(RecipeSO recipeSo)
    {
        recipeNameText.text = recipeSo.recipeName;
        
        foreach (Transform child in iconContainer)
        {
            if (child != iconTemplate)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (var kitchenObjectSo in recipeSo.recipes)
        {
            var instantiated=Instantiate(iconTemplate, iconContainer);
            instantiated.gameObject.SetActive(true);
            var image = instantiated.transform.GetComponent<Image>();
            image.sprite = kitchenObjectSo.sprite;
            
        }
    }
}
