using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ClearCounter : BaseCounter
{
  [SerializeField] private KitchenObjectSO kitchenObjects;
  public override void Interact(Player player)
  {
      if (!HasKitchenObject())
      {
          //There is no KitchenObject here
          if (player.HasKitchenObject())
          {
              //Player is carrying something
              player.GetKitchenObject().SetKitchenObjectParent(this);
          }
      }
      else
      {
          //There is a KitchenObject here
          if (player.HasKitchenObject())
          {
              //Player is carrying something
              if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) 
              {
                  //Player is holding a Plate
                  if ( plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                  {
                    GetKitchenObject().DestroySelf();
                  }
              }
              else
              {
                  //Player isn't carrying Plate but something else
                  if (GetKitchenObject().TryGetPlate(out PlateKitchenObject hasPlateKitchenObject))
                  { 
                      //Counter is holading a Plate
                      if ( hasPlateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO()))
                      {
                          player.GetKitchenObject().DestroySelf();
                      }
                  }
              }
          }
          else
          {
              //Player is not carrying anything
              //Pick up the object
              GetKitchenObject().SetKitchenObjectParent(player);
          }
      }

  }

 
}
