using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript.Aska;
using Script.Managers;
using Script.Managers.Senders;
using Script.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Core.Popups
{
    public class AskaPopup: Popup
    {
        [SerializeField] private Image avatar;
        [SerializeField] private TMP_Text name;
        [SerializeField] private AskaApp aska;        
        [SerializeField] private int stayTimeMilliseconds;
        [SerializeField] private int delayTimeMilliseconds;
        
        private AskaChat chatToRedirect;
        private Button redirectButton;
        private CancellationTokenSource notificationDisappearCts; 
        private Vector3 initialPosition;
        
        private void Start()
        {
            gameObject.SetActive(false);
            initialPosition = gameObject.transform.position;
            redirectButton = GetComponent<Button>();
            redirectButton.onClick.AddListener(RedirectToChat);
            AskaMessageSender.OnNewMessageSend += ShowNotification;
            GameManager.OnGameEnd += CloseApp;
            notificationDisappearCts = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            AskaMessageSender.OnNewMessageSend -= ShowNotification;
            GameManager.OnGameEnd -= CloseApp;

            if (notificationDisappearCts != null)
            {
                notificationDisappearCts.Dispose();
            }
        }

        private void ShowNotification(AskaChat chat)
        {
            if (aska.CurrentOpenedChat == chat && aska.IsOpen)
            {
                return;
            }

            chatToRedirect = chat;
            avatar.sprite = chat.User.ProfilePicture;
            name.text = $"от: {chat.User.Name}";
            
            OpenApp();
        }

        public override void OpenApp()
        {
            OpenAppAsync().Forget();
        }
        private async UniTask OpenAppAsync()
        {
            if (isOpen)
            {
                await UniTask.Delay(delayTimeMilliseconds);
                CloseApp();
            }
            
            base.OpenApp();
            DisappearAfterTimer(notificationDisappearCts.Token).Forget();
            isOpen = true;
        }
        private async UniTask DisappearAfterTimer(CancellationToken token)
        {
            await UniTask.Delay(stayTimeMilliseconds, cancellationToken: token);
            CloseApp();
        }

        public override void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            CloseAppAsync().Forget();
        }
        private async UniTask CloseAppAsync()
        {
            CancelCts();
            await gameObject.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            base.CloseApp();
            isOpen = false;
            gameObject.transform.position = initialPosition;
        }
        private void RedirectToChat()
        {
            CancelCts();
            aska.OpenApp();
            chatToRedirect.OpenChat();
            CloseApp();
        }

        private void CancelCts()
        {
            notificationDisappearCts.Cancel();
            notificationDisappearCts.Dispose();
            notificationDisappearCts = new CancellationTokenSource();
        }
    }
}