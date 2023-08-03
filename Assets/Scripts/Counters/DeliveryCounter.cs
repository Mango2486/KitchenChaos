using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
 
   //Because now we only hava one DeliveryCounter so we can make it an instance
   // it can be modified if more deliveryCounter were added.
   public static DeliveryCounter Instance { get; private set; }

   private void Awake()
   {
      Instance = this;
   }

   public override void Interact(Player player)
   {  
      //Player is holding something
      if (player.HasKitchenObject())
      {  
         //Player is holding a plate
         if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
         {  
            //Only accepts Plates
            DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
            player.GetKitchenObject().DestroySelf();
         }
      }
   }
}
