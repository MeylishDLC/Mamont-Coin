using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core.Popups;
using Script.Managers;
using Script.Sound;
using Script.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Script.Apps.Duralingo
{
    public class DuralingoGame: MonoBehaviour, IWindowedApp
    {
        [SerializeField] private Popup duralingoSpamPopup;
        [SerializeField] private Image timerImage;
        [SerializeField] private TextField textField;
        [SerializeField] private Button SubmitButton;
        [SerializeField] private float animationScale;

        [Header("Screens")] 
        [SerializeField] private GameObject gameScreen;
        [SerializeField] private GameObject loseScreen;
        [SerializeField] private GameObject winScreen;
        
        [Header("Timers")]
        [SerializeField] private int timeToSolveMilliseconds = 60000;
        [SerializeField] private int timeBeforeBombing = 2000;

        private CancellationTokenSource duralingoTimerCts;

        private void Start()
        {
            duralingoTimerCts = new CancellationTokenSource();
            SubmitButton.onClick.AddListener(OnButtonSubmit);
            
            gameScreen.SetActive(true);
            loseScreen.SetActive(false);
            winScreen.SetActive(false);
        }
        

        private void OnDestroy()
        {
            SubmitButton.onClick.RemoveAllListeners();
            duralingoTimerCts.Dispose();
        }

        private async void OnButtonSubmit()
        {
            duralingoTimerCts.Cancel();
            duralingoTimerCts.Dispose();

            SubmitButton.interactable = false;
            
            gameScreen.SetActive(false);
            if (textField.CheckAccuracy())
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.duralingoCorrect);
                winScreen.SetActive(true);
            }
            else
            {
                AudioManager.instance.PlayOneShot(FMODEvents.instance.duralingoWrong);
                loseScreen.SetActive(true);
                await UniTask.Delay(timeBeforeBombing);
                duralingoSpamPopup.PopupAppear();
                
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
            WaitForTimer(duralingoTimerCts.Token).Forget();
        }

        private async UniTask WaitForTimer(CancellationToken token)
        {
            var startTime = Time.time;
            var endTime = startTime + timeToSolveMilliseconds / 1000f;

            while (Time.time < endTime)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }

                var elapsedTime = Time.time - startTime;
                timerImage.fillAmount = elapsedTime / (endTime - startTime);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            if (token.IsCancellationRequested)
            {
                timerImage.fillAmount = 1f;
                loseScreen.SetActive(true);
                await UniTask.Delay(timeBeforeBombing, cancellationToken: token);
                duralingoSpamPopup.PopupAppear();
            }
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