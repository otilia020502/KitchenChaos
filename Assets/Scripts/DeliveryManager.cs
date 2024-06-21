using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using ScriotableObjects;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeliveryManager :NetworkBehaviour
{
    public event EventHandler OnRecipeAdded, OnRecipeCompleted, OnRecipeSucces, OnRecipeFailed;
    
    
    public static DeliveryManager Instance { get; private set;}
    [SerializeField] private RecipeSOList _recipeSoList;
    private List<RecipeSO> waitingRecipes;//client class
    private float spawnRecipeTimer=4f;
    private float spawnRecipeTimerMax = 4f;
    private int waitingRecipesMax = 4;
    private int succesfullDelivery;
    private void Awake()
    {
        Instance = this;
        waitingRecipes = new List<RecipeSO>();
    }

    private void Update()
    {
        if (!IsServer)
        {
            return;
        }
        spawnRecipeTimer -= Time.deltaTime;
        if (spawnRecipeTimer <= 0f)
        {
            spawnRecipeTimer = spawnRecipeTimerMax;
            
            if (KitchenGameManager.Instance.GameIsPlaying() && waitingRecipes.Count < waitingRecipesMax)
            {
                int waitingRecipeSoIndex = Random.Range(0, _recipeSoList.recipes.Count);
                
                SpawnNewWaitingRecipeClientRpc(waitingRecipeSoIndex);
                
            }
        }
    }
    [ClientRpc]
    private void SpawnNewWaitingRecipeClientRpc(int waitingRecipeSOIndex)
    {
        RecipeSO waitingRecipeSo = _recipeSoList.recipes[waitingRecipeSOIndex];
        waitingRecipes.Add(waitingRecipeSo);
        OnRecipeAdded?.Invoke(this, EventArgs.Empty);
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
                    DeliverSuccesfullRecipeServerRpc(i);
                    return;
                    
                }
                
            }
        }
        DeliverFailedRecipeServerRpc();
        //no matches found/ the player did not deliver a correct recipe
        
    }

    [ServerRpc]
    private void DeliverSuccesfullRecipeServerRpc(int waitingRecipeIndex)
    {
        DeliverSuccesfullRecipeClientRpc(waitingRecipeIndex);
    }
    [ClientRpc]
    private void DeliverSuccesfullRecipeClientRpc(int waitingRecipeIndex)
    {
        succesfullDelivery++;
        waitingRecipes.RemoveAt(waitingRecipeIndex);
        OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
        OnRecipeSucces?.Invoke(this, EventArgs.Empty);
        
    }
    [ServerRpc]
    private void DeliverFailedRecipeServerRpc()
    {
        DeliverFailedRecipeClientRpc();
    }
    [ClientRpc]
    private void DeliverFailedRecipeClientRpc()
    {
        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
    }
    public List<RecipeSO> GetWaitingRecipesList()
    {
        return waitingRecipes;
    }

    public int GetSuccesfullRecipes()
    {
        return succesfullDelivery;
    }
}
