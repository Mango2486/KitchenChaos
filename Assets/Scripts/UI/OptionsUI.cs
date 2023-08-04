using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{   
    
    public static OptionsUI Instance { get; private set; }
    
    [SerializeField] private Button soundEffectsButton;
    [SerializeField] private Button musicButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button interactButton;
    [SerializeField] private Button alternateInteractButton;
    [SerializeField] private Button pauseButton;
    [SerializeField] private TextMeshProUGUI soundEffectsText;
    [SerializeField] private TextMeshProUGUI musicText;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private TextMeshProUGUI alternateInteractText;
    [SerializeField] private TextMeshProUGUI pauseText;
    [SerializeField] private Transform pressToRebindKeyTransform;
    private void Awake()
    {
        Instance = this;
        
        soundEffectsButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.ChangeVolume();
            UpdateVisual();
        }); 
        musicButton.onClick.AddListener(() =>
        {
            MusicManager.Instance.ChangeVolume();
            UpdateVisual();
        });
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        
        moveUpButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveUp);
        });   
        moveDownButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveDown);
        });   
        moveLeftButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveLeft);
        });   
        moveRightButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.MoveRight);
        });   
        interactButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Interact);
        });   
        alternateInteractButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.AlternateInteract);
        });  
        pauseButton.onClick.AddListener(() =>
        {
            RebindBinding(GameInput.Binding.Pause);
        });
    }

    private void Start()
    {   
        GameManager.Instance.onGameUnPaused += GameManager_OnGameUnPaused;
        
        UpdateVisual();
        
        HidePressToRebindKey();
        Hide();
    }

    private void GameManager_OnGameUnPaused(object sender, EventArgs e)
    {
        Hide();
    }

    private void UpdateVisual()
    {
        soundEffectsText.text = "Sound Effects: "+Mathf.Round(SoundManager.Instance.GetVolume()*10f);
        musicText.text = "Music: "+Mathf.Round(MusicManager.Instance.GetVolume()*10f);

        moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
        moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
        moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
        moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
        interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
        alternateInteractText.text = GameInput.Instance.GetBindingText(GameInput.Binding.AlternateInteract);
        pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(true);
    }
    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.gameObject.SetActive(false);
    }

    private void RebindBinding(GameInput.Binding binding)
    {
        ShowPressToRebindKey();
       // GameInput.Instance.RebindBinding(binding,ActionRebound);
       //上下效果一样的
       GameInput.Instance.RebindBinding(binding, () =>
       {
           HidePressToRebindKey();
           UpdateVisual();
       });
    }

    private void ActionRebound()
    {
        HidePressToRebindKey();
        UpdateVisual();
    }
        
}
