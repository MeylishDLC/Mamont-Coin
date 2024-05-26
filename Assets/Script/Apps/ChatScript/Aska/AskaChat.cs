using System;
using NUnit.Framework;
using Script.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.ChatScript.Aska
{
    public class AskaChat: MonoBehaviour
    {
        [field: SerializeField] public GameObject MessageContainer { get; private set; }
        [field:SerializeField] public AskaUser User { get; private set; }
        public bool IsOpen { get; private set; }
        
        [SerializeField] private TMP_Text notificationIndicator;
        [SerializeField] private Button OpenChatButton; 

        public static event Action<AskaChat> OnChatChanged;
        private void Awake()
        {
            OpenChatButton.onClick.AddListener(OpenChat);
            notificationIndicator.gameObject.SetActive(false);
            OnChatChanged += CloseChat;
            AskaMessageSender.OnNewMessageSend += SetNotificationIndicator;
            gameObject.SetActive(false);
        }

        public void OpenChat()
        {
            if (IsOpen)
            {
                return;
            }
            
            gameObject.SetActive(true);
            notificationIndicator.gameObject.SetActive(false);
            IsOpen = true;
            OnChatChanged?.Invoke(this);
        }
        private void CloseChat(AskaChat _)
        {
            gameObject.SetActive(false);
            IsOpen = false;
        }
        private void SetNotificationIndicator(AskaChat chat)
        {
            if (chat != this)
            {
                return;
            }

            if (IsOpen)
            {
                notificationIndicator.gameObject.SetActive(false);
            }
            else
            {
                notificationIndicator.gameObject.SetActive(true);
            }

        }
    }
}