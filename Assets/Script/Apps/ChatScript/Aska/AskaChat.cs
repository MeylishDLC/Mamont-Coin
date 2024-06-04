using System;
using Script.Managers.Senders;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Apps.ChatScript.Aska
{
    public class AskaChat: MonoBehaviour
    {
        [field: SerializeField] public GameObject MessageContainer { get; private set; }
        [field:SerializeField] public AskaUser User { get; private set; }
        public bool IsOpen { get; private set; }
        
        [SerializeField] private Image notificationIndicator;
        [SerializeField] private Button OpenChatButton;
        
        [SerializeField] private Color activeProfileColor;
        [SerializeField] private Color inactiveProfileColor;
        [SerializeField] private Image profileImage;
        public static event Action<AskaChat> OnChatChanged;

        private AskaMessageSender askaMessageSender;
        private void Awake()
        {
            OpenChatButton.onClick.AddListener(OpenChat);
            OnChatChanged += CloseChat;
            askaMessageSender.OnNewMessageSend += SetNotificationIndicator;
            
            inactiveProfileColor = profileImage.color;
            
            notificationIndicator.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }

        [Inject]
        public void Construct(AskaMessageSender askaMessageSender)
        {
            this.askaMessageSender = askaMessageSender;
        }

        private void OnDestroy()
        {
            askaMessageSender.OnNewMessageSend -= SetNotificationIndicator;
            OnChatChanged -= CloseChat;
        }

        public void OpenChat()
        {
            if (IsOpen)
            {
                return;
            }
            
            gameObject.SetActive(true);
            notificationIndicator.gameObject.SetActive(false);
            profileImage.color = activeProfileColor;
            
            IsOpen = true;
            OnChatChanged?.Invoke(this);
        }
        private void CloseChat(AskaChat chat)
        {
            if (chat == this)
            {
                return;
            }
            
            gameObject.SetActive(false);
            profileImage.color = inactiveProfileColor;
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