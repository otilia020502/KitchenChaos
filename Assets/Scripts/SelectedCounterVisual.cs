using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   [SerializeField] private ClearCounter clearCounter;
   [SerializeField]private GameObject selectedCounterVisual;
   private void Start()
   {
      Player.Instance.OnSelectedCounterChanged += OnSelectedCounterChanged;
   }

   private void OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
   {
      if (clearCounter == e.SelectedCounter)
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
      selectedCounterVisual.SetActive(true);
   }

   private void Hide()
   {
      selectedCounterVisual.SetActive(false);
   }
}
