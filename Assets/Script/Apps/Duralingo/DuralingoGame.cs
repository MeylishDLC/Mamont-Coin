using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Script.Apps.Duralingo
{
    public class DuralingoGame: MonoBehaviour, IWindowedApp
    {
        
        [SerializeField] private TextField textField;
        [SerializeField] private Button SubmitButton;
        [SerializeField] private float animationScale;

        [Header("Screens")] 
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private GameObject winScreen;
        
        //todo: countdown 

        private void Start()
        {
            SubmitButton.onClick.AddListener(OnButtonSubmit);
            
            gameScreen.SetActive(true);
            loseScreen.SetActive(false);
            winScreen.SetActive(false);
        }

        private void OnDestroy()
        {
            SubmitButton.onClick.RemoveAllListeners();
        }

        private async void OnButtonSubmit()
        {
            SubmitButton.interactable = false;
            
            gameScreen.SetActive(false);
            if (textField.CheckAccuracy())
            {
                winScreen.SetActive(true);
            }
            else
            {
                loseScreen.SetActive(true);
                
            }
            await UniTask.Delay(5000);
            CloseApp();
        }
        
        public void OpenApp()
        {
            OpenAppAsync().Forget();
        }

        private async UniTask OpenAppAsync()
        {
            gameObject.SetActive(true);
            await gameObject.transform.DOScale(animationScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }

        public void CloseApp()
        {
            CloseAppAsync().Forget();
        }

        private async UniTask CloseAppAsync()
        {
            await gameObject.transform.DOScale(animationScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            gameObject.SetActive(false);
        }
    }
}