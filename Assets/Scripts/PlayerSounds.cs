using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
   private Player player;
   private float footsStepTimer;
   private float footsStepTimerMax=.1f;
   private void Awake()
   {
      player = GetComponent<Player>();
   }

   private void Update()
   {
      footsStepTimer -= Time.deltaTime;
      if (footsStepTimer < 0f)
      {
         footsStepTimer = footsStepTimerMax;

         if (player.IsWalking())
         {
            float volume = 1f;
            SoundManager.Instance.PlayFootsStepsSound(player.transform.position, volume);
         }
        
      }
   }
}
