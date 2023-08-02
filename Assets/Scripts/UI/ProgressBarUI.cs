using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
   /*
   before
   [SerializeField] private CuttingCounter cuttingCounter;
   [SerializeField] private Image barImage;
   */
   [SerializeField] private GameObject hasProgressGameObject;
   [SerializeField] private Image barImage;
   
   private IHasProgress hasProgress;

   private void Start()
   {
      hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
      if (hasProgress == null)
      {
         Debug.LogError("Game Object" + hasProgressGameObject + " does not have a component that implements IHasProgress!");
      }
      hasProgress.OnProgressChanged += hasProgress_OnProgressChanged;
      barImage.fillAmount = 0f;
      Hide();
   }

   private void hasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
   {
      barImage.fillAmount = e.progressNormalized;
      if (e.progressNormalized == 0f || Math.Abs(e.progressNormalized-1f) <= Single.Epsilon) 
      {
         Hide();
      }
      else
      {
         Show();
      }
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
