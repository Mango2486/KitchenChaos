using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class GameInput : MonoBehaviour
{
   private const string PLAYER_PREFS_BINDINGS = "InputBindings";
   public static GameInput Instance { get; private set; }
   
   public event EventHandler OnInteractActoin;
   public event EventHandler OnInteractAlternateAction;
   public event EventHandler OnPauseAction;
   public event EventHandler OnBindingRebind;
   public enum Binding
   {
      MoveUp,
      MoveDown,
      MoveLeft,
      MoveRight,
      Interact,
      AlternateInteract,
      Pause,
   }
   
   private PlayerInputActions playerInputActions;
   private void Awake()
   {
      Instance = this;
      // PlayerInputActions won't be destroyed automatically, we should dipose it every time when the GameInput has been destroyed.
      playerInputActions = new PlayerInputActions();
      if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
      {
         playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
      }
      playerInputActions.Player.Enable();
      //performed事件订阅InteractPerformed函数,按下Interact按键就会执行performed事件。
      //订阅方法时不需要加上（）,方法名字即可。
      //InputSystem是实时检测的，所以只需要订阅一次函数即可，不需要考虑把交互放在Update里面。
      playerInputActions.Player.Interact.performed +=　OnInteractPerformed;
      playerInputActions.Player.InteractAlternate.performed += OnInteracteAlternatePerformed;
      playerInputActions.Player.Pause.performed += OnPausePerformed;

   }

   private void OnDestroy()
   {
      
      playerInputActions.Player.Interact.performed -=　OnInteractPerformed;
      playerInputActions.Player.InteractAlternate.performed -= OnInteracteAlternatePerformed;
      playerInputActions.Player.Pause.performed -= OnPausePerformed;
      //InputSystem生成的实例并不会被Unity自动销毁，每次触发GameInput脚本时都会新生成一个实例，这样之前的实例调用之前订阅的事件就会发生引用丢失的问题
      //所以应该手动撤销事件的订阅并且在切换场景之前手动销毁之前生成的InputSystem实例。
      playerInputActions.Dispose();
   }

   private void OnPausePerformed(InputAction.CallbackContext obj)
   {
      OnPauseAction?.Invoke(this,EventArgs.Empty);
   }

   private void OnInteracteAlternatePerformed(InputAction.CallbackContext obj)
   {
      OnInteractAlternateAction?.Invoke(this,EventArgs.Empty); 
   }

   private void OnInteractPerformed(InputAction.CallbackContext obj)
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

   public string GetBindingText(Binding binding)
   {
      switch (binding)
      {
         default:
         case Binding.Interact:
            return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            break;
         case Binding.AlternateInteract:
            return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            break;
         case Binding.Pause:
            return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
            break;
         case Binding.MoveUp:
            return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            break;
         case Binding.MoveDown:
            return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            break;
         case Binding.MoveLeft:
            return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            break;
         case Binding.MoveRight:
            return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            break;
      }
   }
   
   public void RebindBinding(Binding binding, Action onActionRebound)
   {
      playerInputActions.Player.Disable();

      InputAction inputAction;
      int bindingIndex;

      switch (binding)
      {
         default:
         case Binding.MoveUp:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 1;
            break; 
         case Binding.MoveDown:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 2;
            break;
         case Binding.MoveLeft:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 3;
            break;
         case Binding.MoveRight:
            inputAction = playerInputActions.Player.Move;
            bindingIndex = 4;
            break;
         case Binding.Interact:
            inputAction = playerInputActions.Player.Interact;
            bindingIndex = 0;
            break; 
         case Binding.AlternateInteract:
            inputAction = playerInputActions.Player.InteractAlternate;
            bindingIndex = 0;
            
            break; 
         case Binding.Pause:
            inputAction = playerInputActions.Player.Pause;
            bindingIndex = 0;
            break;
      }

      inputAction.PerformInteractiveRebinding(bindingIndex)
         .OnComplete(callback =>
         {
            callback.Dispose();
            playerInputActions.Player.Enable();
            onActionRebound();

            playerInputActions.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
            
            OnBindingRebind?.Invoke(this,EventArgs.Empty);
         })
         .Start();

   }
}
