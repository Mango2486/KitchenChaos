using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour
{
   private const string POPUP = "Popup";
   
   [SerializeField] private Image backgroundImage;
   [SerializeField] private Image iconImage;
   [SerializeField] private TextMeshProUGUI deliveryMessageText;
   [SerializeField] private Color successColor;
   [SerializeField] private Color failedColor;
   [SerializeField] private Sprite successSprite;
   [SerializeField] private Sprite failedSprite;


   private Animator animator;

   private void Awake()
   {
      animator = GetComponent<Animator>();
   }

   private void Start()
   {
      DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
      DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
      
      gameObject.SetActive(false);
   }

   private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
   {  
      gameObject.SetActive(true);
      animator.SetTrigger(POPUP);
      backgroundImage.color = failedColor;
      iconImage.sprite = failedSprite;
      deliveryMessageText.text = "DELIVERY\nFAILD";
   }

   private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
   {
      gameObject.SetActive(true);
      animator.SetTrigger(POPUP);
      backgroundImage.color = successColor;
      iconImage.sprite = successSprite;
      deliveryMessageText.text = "DELIVERY\nSUCCESS";
   }
}
