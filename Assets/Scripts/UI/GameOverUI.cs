using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI deliveredRecipesText;
        // i dont have in awake networkmanager.singleton.shutdown, because i dont have a button to load the scene
    private void Start()
    {
        KitchenGameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (KitchenGameManager.Instance.IsGameOver())
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false); 
        
    }

    private void Show()
    {
        gameObject.SetActive(true);
        deliveredRecipesText.text = DeliveryManager.Instance.GetSuccesfullRecipes().ToString();
    }

    

}
