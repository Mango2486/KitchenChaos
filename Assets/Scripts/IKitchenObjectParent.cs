using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
    public Transform GetKitchenObjectFollowTransform();
    public KitchenObjects GetKitchenObject();
    public void SetKitchenObject(KitchenObjects kitchenObject); 
    public void ClearKitchenObject();
    public bool HasKitchenObject();

}
