using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameClockUI : MonoBehaviour
{
    [SerializeField] private Image clockImage;

    private void Update()
    {
        clockImage.fillAmount = KitchenGameManager.Instance.GetPlayingTimerNormalized();
    }
}
