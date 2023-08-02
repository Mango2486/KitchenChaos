using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
 
   
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
