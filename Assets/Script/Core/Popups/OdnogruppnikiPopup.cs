using System;
using Cysharp.Threading.Tasks;
using Script.Apps.SmallStuff.AppsOnWorkspace.InternetExplorer;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Core.Popups
{
    public class OdnogruppnikiPopup: Popup
    { 
        [SerializeField] private Sprite[] notificationVariations;
        [SerializeField] private ExplorerApp explorerApp;
        [SerializeField] private Tab tabToOpen;
        [SerializeField] private int appearIntervalMilliseconds;
        
        private Button redirectButton;
        private Image notificationImage;

        private void Start()
        {
            gameObject.SetActive(false);
            redirectButton = GetComponent<Button>();
            notificationImage = GetComponent<Image>();
            
            redirectButton.onClick.AddListener(RedirectToTab);
            notificationImage.sprite = notificationVariations[0];
            AppearWithInterval().Forget();
        }
        private async UniTask AppearWithInterval()
        {
            await UniTask.Delay(appearIntervalMilliseconds);
            for (int i = 0; i < notificationVariations.Length; i++)
            {
                notificationImage.sprite = notificationVariations[i];
                OpenApp();
                await UniTask.Delay(appearIntervalMilliseconds);
            }
        }

        private void RedirectToTab()
        {
            explorerApp.OpenApp();
            explorerApp.OpenTab(tabToOpen);
            CloseApp();
        }
    }
}