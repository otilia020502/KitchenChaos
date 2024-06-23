using System;
using System.Collections;
using System.Collections.Generic;
using Counters;
using UnityEngine;
using UnityEngine.Serialization;
using GameObject = UnityEngine.GameObject;

public class SelectedCounterVisual : MonoBehaviour
{
   [FormerlySerializedAs("clearCounter")] [SerializeField] private BaseCounter currentCounter;
   [SerializeField]private GameObject[] selectedCounterVisual;
   private void Start()
   {
      if (Player.LocalInstance != null)
      {
         Player.LocalInstance.OnSelectedCounterChanged += OnSelectedCounterChanged;
         
      }
      else
      {
         Player.OnAnyPlayerSpawned += PlayerSpawned;
      }
      
   }

   private void PlayerSpawned(object sender, EventArgs e)
   {
     
      if (Player.LocalInstance != null)
      {
         Player.LocalInstance.OnSelectedCounterChanged -= OnSelectedCounterChanged;
         Player.LocalInstance.OnSelectedCounterChanged += OnSelectedCounterChanged;
      }
      
   }

   private void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
   {
      if (e.SelectedCounter == currentCounter)
      {
         Show();
      }
      else
      {
         Hide();
      }
   }

   private void Show()
   {
      foreach (GameObject visualGameObject in selectedCounterVisual)
      {
         visualGameObject.SetActive(true);
      }
      
   }

   private void Hide()
   {
      foreach (GameObject visualGameObject in selectedCounterVisual)
      {
         visualGameObject.SetActive(false);
      }
   }
}
