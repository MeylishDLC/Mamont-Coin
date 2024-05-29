using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core.Popups;
using Script.Core.Popups.Spawns;
using Script.Managers;
using Script.Sound;
using Script.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Script.Apps.Duralingo
{
    public class DuralingoGame: MonoBehaviour, IWindowedApp
    {
        [SerializeField] private CallSpawner duralingoSpamSpawner;
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
        private AudioManager audioManager;
        private FMODEvents FMODEvents;

        private bool isOpen;

        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        private void Start()
        {
            duralingoTimerCts = new CancellationTokenSource();
            SubmitButton.onClick.AddListener(OnButtonSubmit);
            GameManager.OnGameEnd += CloseApp;
            
            gameScreen.SetActive(true);
            loseScreen.SetActive(false);
            winScreen.SetActive(false);
        }
        
        private void OnDestroy()
        {
            SubmitButton.onClick.RemoveAllListeners();
            duralingoTimerCts?.Dispose();
            GameManager.OnGameEnd -= CloseApp;
        }

        private async void OnButtonSubmit()
        {
            duralingoTimerCts.Cancel();
            duralingoTimerCts.Dispose();

            SubmitButton.interactable = false;
            
            gameScreen.SetActive(false);
            if (textField.CheckAccuracy())
            {
                audioManager.PlayOneShot(FMODEvents.duralingoCorrect);
                winScreen.SetActive(true);
            }
            else
            {
                audioManager.PlayOneShot(FMODEvents.duralingoWrong);
                loseScreen.SetActive(true);
                await UniTask.Delay(timeBeforeBombing);
                duralingoSpamSpawner.StartSpawn();
            }
            await UniTask.Delay(5000);
            CloseApp();
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
            await gameObject.transform.DOScale(animationScale, 0.1f)
                .SetLoops(2, LoopType.Yoyo).ToUniTask();
            isOpen = true;
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

            if (!token.IsCancellationRequested)
            {
                timerImage.fillAmount = 1f;
                loseScreen.SetActive(true);
                await UniTask.Delay(timeBeforeBombing, cancellationToken: token);
                duralingoSpamSpawner.StartSpawn();
            }
        }
        public void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            CloseAppAsync().Forget();
        }

        private async UniTask CloseAppAsync()
        {
            await gameObject.transform.DOScale(animationScale, 0.1f)
                .SetLoops(2, LoopType.Yoyo).ToUniTask();
            isOpen = false;
            gameObject.SetActive(false);
        }
    }
}