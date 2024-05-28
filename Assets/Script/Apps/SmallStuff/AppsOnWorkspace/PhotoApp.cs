using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace
{
    public class PhotoApp: BasicWorkspaceApp
    {
        [field:SerializeField] public TMP_Text PhotoText { get; private set; }
        [field:SerializeField] public Image Picture { get; private set; }
        protected override void Start()
        {
            GameManager.OnGameEnd += CloseApp;
            closeButton.onClick.AddListener(CloseApp);
            
            gameObject.transform.localScale = new Vector3(scaleOnClose, scaleOnClose, 0);
            initPos = gameObject.transform.localPosition;
            
            gameObject.SetActive(false);
        }

   
        public override void OpenApp()
        {
            gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);

            OpenAppAsync().Forget();
        }

        public override void CloseApp()
        {
            CloseAppAsync().Forget();
        }

        private async UniTask CloseAppAsync()
        {
            closeButton.interactable = false;
            
            await gameObject.transform.DOScale(scaleOnClose, scaleDuration).ToUniTask();
            gameObject.SetActive(false);
            IsOpen = false;
            gameObject.transform.localPosition = initPos;
            closeButton.interactable = true;
        }
        private async UniTask OpenAppAsync()
        {
            closeButton.interactable = false;
            
            gameObject.SetActive(true);
            await gameObject.transform.DOScale(1, scaleDuration).ToUniTask();
            IsOpen = true;
            
            closeButton.interactable = true;
        }
    }
}