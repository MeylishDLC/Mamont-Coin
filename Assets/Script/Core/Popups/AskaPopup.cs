﻿using System;
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
        private AskaMessageSender askaMessageSender;
        
        private void Awake()
        {
            initialPosition = gameObject.transform.position;
            
            redirectButton = GetComponent<Button>();
            redirectButton.onClick.AddListener(RedirectToChat);
            closeButton.onClick.AddListener(CloseApp);
            
            askaMessageSender.OnNewMessageSend += ShowNotification;
            GameManager.OnGameEnd += CloseApp;
            notificationDisappearCts = new CancellationTokenSource();
            
            gameObject.SetActive(false);
        }

        [Inject]
        public void Construct(AskaMessageSender askaMessageSender)
        {
            this.askaMessageSender = askaMessageSender;
        }

        private void OnDestroy()
        {
            askaMessageSender.OnNewMessageSend -= ShowNotification;
            GameManager.OnGameEnd -= CloseApp;
            notificationDisappearCts?.Dispose();
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
            
            gameObject.SetActive(true);
            isOpen = true;
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
            
            DisappearAfterTimer(notificationDisappearCts.Token).Forget();
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
            isOpen = false;
            await gameObject.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            gameObject.SetActive(false);
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