using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Script.Data;
using TMPro;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool destroyOnClose;
    public bool isPaid;

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
