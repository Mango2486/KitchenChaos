using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI keyMoveUpText;
   [SerializeField] private TextMeshProUGUI keyMoveDownText;
   [SerializeField] private TextMeshProUGUI keyMoveLeftText;
   [SerializeField] private TextMeshProUGUI keyMoveRightText;
   [SerializeField] private TextMeshProUGUI keyMoveInteractText;
   [SerializeField] private TextMeshProUGUI keyMoveInteractAlternateText;
   [SerializeField] private TextMeshProUGUI ketMovePauseText;


   private void Start()
   {
      GameInput.Instance.OnBindingRebind += GameInput_OnBindingRebind;
      GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
      UpdateVisual();
      Show();
   }

   private void GameManager_OnStateChanged(object sender, EventArgs e)
   {
      if (GameManager.Instance.IsCountdownToStartActive())
      {
         Hide();
      }
   }

   private void GameInput_OnBindingRebind(object sender, EventArgs e)
   {
      UpdateVisual();
   }

   private void UpdateVisual()
   {
      keyMoveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
      keyMoveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
      keyMoveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
      keyMoveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
      keyMoveInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
      keyMoveInteractAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.AlternateInteract);
      ketMovePauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
   }

   private void Show()
   {
      gameObject.SetActive(true);
   }

   private void Hide()
   {
      gameObject.SetActive(false);
   }
}
