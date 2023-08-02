using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class KitchenObjects : MonoBehaviour
{
   [SerializeField] private KitchenObjectSO kitchenObjectSO;

   private IKitchenObjectParent kitchenObjectParent;

   public KitchenObjectSO GetKitchenObjectSO()
   {
      return kitchenObjectSO;
   }

   public IKitchenObjectParent GetKitchenObjectParent()
   {
      return this.kitchenObjectParent;

   }

   public void SetKitchenObjectParent(IKitchenObjectParent kitchenObjectParent)
   {
      //if  object  has a kitchenObjectParent, then set the kitchenObjectParent's object to null
      if (this.kitchenObjectParent != null)
      {
         this.kitchenObjectParent.ClearKitchenObject();
      }

      //Then set object's kitchenObjectParent to the target counter's topPoint.
      this.kitchenObjectParent = kitchenObjectParent;
      if (!kitchenObjectParent.HasKitchenObject())
      {
         kitchenObjectParent.SetKitchenObject(this);
      }
      else
      {
         Debug.LogError("IKitchenObjectParent already has a KitchenObject!");
      }

      //Because we've already set the parent of kitchenObjects, so we can move the object by changing its parent. The parent of object is topPosition in kitchenObjectParent.
      //Change current object's parent and then the position of the object will be changed automatically.
      transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
      //Make sure the object's position is right.
      transform.localPosition = Vector3.zero;
   }

   public void DestroySelf()
   {
      kitchenObjectParent.ClearKitchenObject();
      Destroy(gameObject);
   }

   
   public static KitchenObjects SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
   {
      Transform kitchenObjectTransfrom = Instantiate(kitchenObjectSO.prefab);
      KitchenObjects kitchenObject = kitchenObjectTransfrom.GetComponent<KitchenObjects>();
      kitchenObject.SetKitchenObjectParent(kitchenObjectParent);
      return kitchenObject;
   }

   public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
   {
      if (this is PlateKitchenObject)
      {
         plateKitchenObject = this as PlateKitchenObject;
         return true;
      }
      else
      {
         plateKitchenObject = null;
         return false;
      }
   }
}
