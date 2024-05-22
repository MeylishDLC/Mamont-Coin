using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using Script.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class RankNotificationWindow: MonoBehaviour, IWindowedApp
    {
        [SerializeField] private float scaleOnAppear;
        [SerializeField] private float scaleDuration;
        [SerializeField] private Button closeButton;
        [SerializeField] private Image rankNameBaseImage;
        [SerializeField] private ParticleSystem particlesOnAppear;
        private void Start()
        {
            closeButton.onClick.AddListener(CloseApp);
            ProgressHandler.OnNewMamontTitleReached += OpenWindow;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            ProgressHandler.OnNewMamontTitleReached -= OpenWindow; 
        }

        private async UniTask OpenWindowAsync(Sprite rankSprite)
        {
            rankNameBaseImage.sprite = rankSprite;
            rankNameBaseImage.gameObject.SetActive(true);
            
            gameObject.SetActive(true);
            ParticleSpawn();
            closeButton.interactable = false;
            await gameObject.transform.DOScale(scaleOnAppear, scaleDuration).SetLoops(2, LoopType.Yoyo).ToUniTask();
            closeButton.interactable = true;
        } 
        
        private async UniTask CloseWindowAsync()
        {
            closeButton.interactable = false;
            await gameObject.transform.DOScale(scaleOnAppear, scaleDuration).SetLoops(2, LoopType.Yoyo).ToUniTask();
            gameObject.SetActive(false);
        }

        private void OpenWindow(string _, Sprite rankSprite)
        {
            OpenWindowAsync(rankSprite).Forget();
        }
        public void OpenApp()
        {
            //
        }

        public void CloseApp()
        {
            CloseWindowAsync().Forget();
        }

        private void ParticleSpawn()
        {
            Instantiate(particlesOnAppear, gameObject.GetComponent<RectTransform>().position,
                Quaternion.identity);
        }
    }
}