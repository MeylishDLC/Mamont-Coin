using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnClose;
    [SerializeField] private Button closeButton;
    public bool isPaid;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseWindow);
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
        transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    public void CloseWindow()
    {
        if (destroyOnClose)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
        if (isPaid)
        {
            DataBank.Clicks += BoostsManager.GetInstance().coinsPerPopupWindow;
            Events.ClicksUpdated?.Invoke();
            Debug.Log("+ money for AD");
        }
    }
}
