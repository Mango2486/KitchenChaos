using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour,IKitchenObjectParent
{
    [SerializeField] private Transform topPoint;
    private KitchenObjects kitchenObject;
   public virtual void Interact(Player player)
   {
       Debug.LogError("BaseCounter.Interact();");
   }
   public virtual void InteractAlternate(Player player)
   {
      // Debug.LogError("BaseCounter.InteractAlternate();");
   }

   public Transform GetKitchenObjectFollowTransform()
   {
       return topPoint;
   }

   public KitchenObjects GetKitchenObject()
   {
       return kitchenObject;
   }

   public void SetKitchenObject(KitchenObjects kitchenObject)
   {
       this.kitchenObject = kitchenObject;
   }

   public void ClearKitchenObject()
   {
       kitchenObject = null;
   }

   public bool HasKitchenObject()
   {
       return kitchenObject != null;
   }
}
