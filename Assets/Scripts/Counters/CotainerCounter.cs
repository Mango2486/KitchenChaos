using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CotainerCounter : BaseCounter
{
    public event EventHandler OnPlayerGrabbedObject;
    [SerializeField] private KitchenObjectSO kitchenObjects;

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject())
        {
            //When the method is called, it respawns an object and gives it to he player.
            KitchenObjects.SpawnKitchenObject(kitchenObjects, player);
            OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
        }

    }
}
