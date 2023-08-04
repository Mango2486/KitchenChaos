using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipesDeliveredText;
    
    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        
        Hide();
    }

    private void GameManager_OnStateChanged(object sender, EventArgs e)
    {
        if (GameManager.Instance.IsGameOver())
        {
            Show();
            recipesDeliveredText.text = DeliveryManager.Instance.GetSuccussfulRecipesAmount().ToString();
            StartCoroutine(nameof(BackToMainCorutine));
        }
        else
        {
            Hide();
        }
    }
    

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    IEnumerator BackToMainCorutine()
    {
        yield return new WaitForSeconds(3f);
        Loader.Load(Loader.Scene.MainMenuScene);
    }
}

