using System;
using System.Collections;
using System.Collections.Generic;
using ScriotableObjects;
using UnityEngine;
using Random = UnityEngine.Random;


public class DeliveryManager : MonoBehaviour
{
    [SerializeField] private RecipeSOList _recipeSoList;
    private List<RecipeSO> waitingRecipes;
    private float spawnRecipeTimer;
    private float spawnRecipeTimerMax = 4f;

    private void Update()
    {
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            RecipeSO waitingRecipeSo = _recipeSoList.recipes[Random.Range(0, _recipeSoList.recipes.Count)];
            waitingRecipes.Add((waitingRecipeSo));
        }
    }
}
