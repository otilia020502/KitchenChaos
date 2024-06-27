using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingForOtherPlayersUI : MonoBehaviour
{
   
    void Start()
    {
        Hide();
        KitchenGameManager.Instance.OnPlayerReady += WaitingForOtherPlayersUI_OnPlayerReady;
    }

    private void WaitingForOtherPlayersUI_OnPlayerReady(object sender, EventArgs e)
    {
        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
