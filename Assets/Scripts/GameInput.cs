using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
   public event EventHandler OnInteractActoin;
   public event EventHandler OnInteractAlternateAction;
   private PlayerInputActions playerInputActions;
   private void Awake()
   {
      playerInputActions = new PlayerInputActions();
      playerInputActions.Player.Enable();
      //performed事件订阅InteractPerformed函数,按下Interact按键就会执行performed事件。
      //订阅方法时不需要加上（）,方法名字即可。
      //InputSystem是实时检测的，所以只需要订阅一次函数即可，不需要考虑把交互放在Update里面。
      playerInputActions.Player.Interact.performed += InteractPerformed;
      playerInputActions.Player.InteractAlternate.performed += InteracteAlternatePerformed;
   }

   private void InteracteAlternatePerformed(InputAction.CallbackContext obj)
   {
      OnInteractAlternateAction?.Invoke(this,EventArgs.Empty); 
   }

   private void InteractPerformed(InputAction.CallbackContext obj)
   {
      //两个参数，一个为事件的发送者，一个是带上的参数。
      OnInteractActoin?.Invoke(this, EventArgs.Empty);
   }

   public Vector2 GetMovementVectorNormalized()
   {
      Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
      inputVector = inputVector.normalized;
      return inputVector;
   }
}
