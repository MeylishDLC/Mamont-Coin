using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.Mixer
{
    public class MixerApp : MonoBehaviour, IWindowedApp
    {
        [SerializeField] private Button closeButton;
        [SerializeField] private float scaleOnOpen;
        private Vector2 initialPosition;
        private bool isOpen;
        
        private void Start()
        {
            GameManager.OnGameEnd += CloseApp;

            gameObject.SetActive(false);
            initialPosition = gameObject.transform.position;
            closeButton.onClick.AddListener(CloseApp);
        }

        private void OnDestroy()
        {
            GameManager.OnGameEnd -= CloseApp;
        }

        public void OpenApp()
        {
            if (isOpen)
            {
                return;
            }
            OpenAppAsync().Forget();
        }
        private async UniTask OpenAppAsync()
        {
            gameObject.SetActive(true);
            closeButton.interactable = false;
            await gameObject.transform.DOScale(scaleOnOpen, 0.1f).SetLoops(2, LoopType.Yoyo);
            closeButton.interactable = true;
            isOpen = true;
        }

        public void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            gameObject.SetActive(false);
            gameObject.transform.position = initialPosition;
            isOpen = false;
        }
    }
}
