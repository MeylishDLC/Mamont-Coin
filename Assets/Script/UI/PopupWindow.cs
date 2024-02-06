using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private TextMeshProUGUI info;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowWindow()
    {
        gameObject.SetActive(true);
        transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
}
