using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;


    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounte_OnProgressChanged;
        Hide();
    }

    private void StoveCounte_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();

        if (show)
        {
            Show();
        }
        else
        {
            Hide();
        }

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

