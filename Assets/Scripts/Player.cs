using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour,IKitchenObjectParent
{
   
   //Simple Singleton 
   public static Player Instance
   {
      get;
      private set;
   }

   public event EventHandler OnPickedSomething;
   
   public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
   //这个class是OnSelectedCounterChanged事件的专门数据类，命名就是事件名+EventArgs。同时类继承EventArgs
   public class OnSelectedCounterChangedEventArgs: EventArgs
   
   {
      public BaseCounter selectedCounter;
   }
   
   [SerializeField] private float moveSpeed = 7f;
   [SerializeField] private float rotateSpeed = 10f;
   [SerializeField] private GameInput gameInput; 
   [SerializeField] private LayerMask countersLayer;
   [SerializeField] private Transform topPoint;
   
   
   private bool isWalking;
   private Vector3 lastInteractionDirction;
   private BaseCounter selectedCounter;
   private KitchenObjects kitchenObject;

   

   private void Awake()
   {
      if (Instance != null)
      {
         Debug.LogError("There is more than one Player Instance");
      }
      Instance = this;
   }

   private void Start() 
   {
      gameInput.OnInteractActoin += GameInput_OnInteractActoin;
      gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
   }

   private void GameInput_OnInteractAlternateAction(object sender, EventArgs e)
   {
      if (!GameManager.Instance.IsGamePlaying())
         return;
      if (selectedCounter != null)
      {
         selectedCounter.InteractAlternate(this);
      }
   }

   private void GameInput_OnInteractActoin(object sender, EventArgs e)
   {  
      if (!GameManager.Instance.IsGamePlaying())
         return;
      if (selectedCounter != null)
      {
         selectedCounter.Interact(this);
      }
   }

   private void Update()
   {
      HandleMovement();
      HandleInteractions();
   }

   public bool IsWalking()
   {
      return isWalking;
   }

   private void HandleInteractions()
   {  
      Vector2 inputVector = gameInput.GetMovementVectorNormalized();
      Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
      if (moveDirection != Vector3.zero)
      {
         lastInteractionDirction = moveDirection;
      }
      float interactDistance = 2f;

      if (Physics.Raycast(transform.position, lastInteractionDirction,out RaycastHit hit, interactDistance,countersLayer))
      {
         if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
         {
            // Has ClearCounter
            // Then change _selectedCounter to the clearCounter.
            if (baseCounter != selectedCounter)
            {
               SetSelectedCounter(baseCounter);
            }
         }
         // Has detected counter but the counter don't have ClearCounter
         // Then change _selectedCounter to the clearCounter.
         else
         {
            SetSelectedCounter(null);
         }
      }
      // Hasn't detected any counter. 
      else
      {
         SetSelectedCounter(null);
      }
   }
   
   //PlayerMovement
   private void HandleMovement()
   {
      Vector2 inputVector = gameInput.GetMovementVectorNormalized();

      Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);
      float moveDistance = moveSpeed * Time.deltaTime;
      float playerRadius= .7f;
      float playerHeight = 2f;
      //胶囊射线体s
      bool canMove = !Physics.CapsuleCast(transform.position,transform.position + Vector3.up * playerHeight, playerRadius,moveDirection,moveDistance);

      if (!canMove)
      {
         // Can't move towards moveDirection
         //Attempt only X movement
         Vector3 moveDirectionX = new Vector3(moveDirection.x, 0f, 0f).normalized;
         canMove = moveDirection.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
            playerRadius, moveDirectionX, moveDistance);
         if (canMove)
         {
            //Can move only on the X
            moveDirection = moveDirectionX;
         }
         else
         {
            //Can't move only on the X
            //Attempt only Z movement
            Vector3 moveDirectionZ = new Vector3(0f, 0f, moveDirection.z).normalized;
            canMove = moveDirection.z!= 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight,
               playerRadius, moveDirectionZ, moveDistance);
            if (canMove)
            {
               //Can move only on the Z
               moveDirection = moveDirectionZ;
            }

         }

      }
      if (canMove)
      {
         transform.position += moveDirection * moveDistance;
      }

      isWalking = moveDirection != Vector3.zero;

      transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);

   }

   private void SetSelectedCounter(BaseCounter selectedCounter)
   {  
      //Change selectedCounter to the clearCounter.
      this.selectedCounter = selectedCounter;
      OnSelectedCounterChanged?.Invoke(this,new OnSelectedCounterChangedEventArgs
      {  
         //Set selectedCounter which is in the OnSelectedCounterChangedEventArgs field as selectedCounter
         selectedCounter = selectedCounter
      });
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
      if (kitchenObject != null )
      {
         OnPickedSomething?.Invoke(this,EventArgs.Empty);
      }
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
