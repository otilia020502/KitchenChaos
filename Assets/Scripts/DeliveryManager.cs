using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using ScriotableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager : MonoBehaviour
{
    
    public static DeliveryManager Instance { get; private set;}
    [SerializeField] private RecipeSOList _recipeSoList;
    private List<RecipeSO> waitingRecipes;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private void Awake()
    {
        Instance = this;
        waitingRecipes = new List<RecipeSO>();
    }

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            if (waitingRecipes.Count < waitingRecipesMax)
            {
                RecipeSO waitingRecipeSo = _recipeSoList.recipes[Random.Range(0, _recipeSoList.recipes.Count)];
                Debug.Log(waitingRecipeSo.recipeName);
                waitingRecipes.Add(waitingRecipeSo);
            }
        }
    }

    public void DeliverRecipe(PlatekitchenObject platekitchenObject)
    {
        for (int i = 0; i < waitingRecipes.Count; i++)
        {
            RecipeSO waitingRecipeSo = waitingRecipes[i];

            if (waitingRecipeSo.recipes.Count == platekitchenObject.GetIngridientSos().Count)
            {
                bool plateContentMatchesRecipe = true;
                foreach (KitchenObjectSo recipeKitchenObjectSo in waitingRecipeSo.recipes)
                {
                    bool ingredientFound = false;
                    foreach (KitchenObjectSo plateKitchenObjectSo in platekitchenObject.GetIngridientSos())
                    {
                        if (plateKitchenObjectSo == recipeKitchenObjectSo)
                        {
                            ingredientFound = true;
                            break;
                        }
                        
                    }

                    if (!ingredientFound)
                    {
                        plateContentMatchesRecipe = false;
                    }
                }

                if (plateContentMatchesRecipe)
                {
                    //playerdelivered the correct recipe
                    Debug.Log("player delivered the correct recipe");
                    waitingRecipes.RemoveAt(i);
                    return;
                }
            }
        }
        //no matches found/ the player did not deliver a correct recipe
    }
}
