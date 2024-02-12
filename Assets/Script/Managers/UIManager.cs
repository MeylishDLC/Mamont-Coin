using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Mamont Title")] 
    [SerializeField] private GameObject mamontTitleObject;
    [SerializeField] private float mamontTitleScale;
    private TextMeshProUGUI mamontTitleText;

    [Header("Buffs shop")] [SerializeField]
    private GameObject shopBuffsPanel;

    [SerializeField] private Button showBuffsPanelButton;
    [SerializeField] private Button hideBuffsPanelButton;
    [SerializeField] private float xMoveShow;
    [SerializeField] private float xMoveHide;
    private bool buffsPanelOpen;

    private void Start()
    {
        buffsPanelOpen = false;
        shopBuffsPanel.SetActive(false);

        mamontTitleText = mamontTitleObject.GetComponent<TextMeshProUGUI>();
        mamontTitleText.text = "мамонтёнок";
    }

    private async UniTask ShowPanelAsync()
    {
        Events.ClicksUpdated?.Invoke();
        
        if (!buffsPanelOpen)
        {
            shopBuffsPanel.SetActive(true);
            showBuffsPanelButton.interactable = false;

            await shopBuffsPanel.transform.DOLocalMoveX(xMoveShow, 0.5f).ToUniTask();

            showBuffsPanelButton.interactable = true;
            buffsPanelOpen = true;
        }
    }
    

    public void UpdateMamontTitle(string titleName)
    {
        mamontTitleText.text = titleName;
        mamontTitleObject.transform.DOScale(mamontTitleScale, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    private async UniTask HidePanelAsync()
    {
        hideBuffsPanelButton.interactable = false;
        showBuffsPanelButton.interactable = false;

        await shopBuffsPanel.transform.DOLocalMoveX(xMoveHide, 0.5f).ToUniTask();

        hideBuffsPanelButton.interactable = true;
        showBuffsPanelButton.interactable = true;

        buffsPanelOpen = false;
        shopBuffsPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        ShowPanelAsync().Forget();
    }

    public void HidePanel()
    {
        HidePanelAsync().Forget();
    }

   

}

