using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core;
using Script.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.Clicker
{
    public class BuffsShop: MonoBehaviour
    {
        [SerializeField] private GameObject shopBuffsPanel;
        [SerializeField] private Button showBuffsPanelButton;
        [SerializeField] private Button hideBuffsPanelButton;
        [SerializeField] private float xMoveShow;
        [SerializeField] private float xMoveHide;
        private bool buffsPanelOpen;

        private void Start()
        {
            buffsPanelOpen = false;
            shopBuffsPanel.SetActive(false);
        
            showBuffsPanelButton.onClick.AddListener(ShowPanel);
            hideBuffsPanelButton.onClick.AddListener(HidePanel);
        }

        private void OnDestroy()
        {
            showBuffsPanelButton.onClick.RemoveAllListeners();
            hideBuffsPanelButton.onClick.RemoveAllListeners();
        }

        private async UniTask ShowPanelAsync()
        {
            ClickHandler.ClicksUpdated?.Invoke(0);

            if (buffsPanelOpen)
            {
                HidePanel();
            }
            else
            {
                shopBuffsPanel.SetActive(true);
                showBuffsPanelButton.interactable = false;

                await shopBuffsPanel.transform.DOMoveX(xMoveShow, 0.5f).ToUniTask();

                showBuffsPanelButton.interactable = true;
                buffsPanelOpen = true;
            }
        }
    
        private async UniTask HidePanelAsync()
        {
            hideBuffsPanelButton.interactable = false;
            showBuffsPanelButton.interactable = false;

            await shopBuffsPanel.transform.DOMoveX(xMoveHide, 0.5f).ToUniTask();

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
}