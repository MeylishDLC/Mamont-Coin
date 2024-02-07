using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isInstantiable;
    [SerializeField] private bool destroyOnClose;

    private void Awake()
    {
        if (!isInstantiable)
        {
            gameObject.SetActive(false);
        }
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
    }
}
