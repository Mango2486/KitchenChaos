using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverManagerUI : MonoBehaviour
{
   [SerializeField] private Transform container;
   [SerializeField] private Transform recipeTemplate;


   private void Awake()
   {
      recipeTemplate.gameObject.SetActive(false);
   }

   private void Start()
   {
      DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
      DeliveryManager.Instance.OnRecipeCompleted += DeliveryManager_OnRecipeCompleted;
      UpdateVisual();
   }

   private void DeliveryManager_OnRecipeCompleted(object sender, EventArgs e)
   {
      UpdateVisual();
   }

   private void DeliveryManager_OnRecipeSpawned(object sender, EventArgs e)
   {
      UpdateVisual();
   }

   private void UpdateVisual()
   {
      //可能不太好 每次更新相当于删掉之前所有的存储再重新获取WaitingRecipeSOList里面的内容重新生成。
      foreach (Transform child in container)
      {
         if (child == recipeTemplate)
            continue;
         Destroy(child.gameObject);
      }

      foreach (RecipeSO recipeSO in DeliveryManager.Instance.getWatingRecipeSOList())
      {
         Transform recipeTransform = Instantiate(recipeTemplate, container);
         recipeTransform.gameObject.SetActive(true);
         recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
      }
   }
}
