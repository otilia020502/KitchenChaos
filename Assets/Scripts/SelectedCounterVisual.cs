using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class NewBehaviourScript : MonoBehaviour
{
   [FormerlySerializedAs("clearCounter")] [SerializeField] private BaseCounter currentCounter;
   [SerializeField]private GameObject[] selectedCounterVisual;
   private void Start()
   {
      Player.Instance.OnSelectedCounterChanged += OnSelectedCounterChanged;
   }

   private void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
   {
      foreach (var selectedVisual in selectedCounterVisual)
      {
         selectedVisual.SetActive(e.SelectedCounter);
         
      }
   }

   
}
