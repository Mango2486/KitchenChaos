using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

   [SerializeField] private BaseCounter baseCounter;
   [SerializeField] private GameObject[] visualGameObjectArray;
   
   
   private void Start()
   {
      Player.Instance.OnSelectedCounterChanged += Player_OnOnSelectedCounterChanged;
   }

   private void Player_OnOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
   {  
      // Compare to the baseCounter which is set
      if (e.selectedCounter == baseCounter)
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
      foreach (GameObject visualGameObject in visualGameObjectArray)
      {
         visualGameObject.SetActive(true);
      }
      
   }

   private void Hide()
   {
      foreach (GameObject visualGameObject in visualGameObjectArray)
      {
         visualGameObject.SetActive(false);
      }
   }
   
}
