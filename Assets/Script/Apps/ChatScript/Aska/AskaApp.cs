using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Managers.Senders;
using Script.Sound;
using Script.UI;
using TMPro;
using UnityEngine;
using Zenject;
using UnityEngine.UI;

namespace Script.Apps.ChatScript.Aska
{
    public class AskaApp: MonoBehaviour, IWindowedApp
    {
        public AskaChat CurrentOpenedChat { get; private set; }
        public bool IsOpen { get; private set; }
        [field:SerializeField] public AskaChat[] Chats { get; private set; }
        
        [Header("Settings")] 
        [field: SerializeField] public float MessageScale;

        [Header("Main UI")] 
        [SerializeField] private Button openIcon;
        [SerializeField] private Button closeButton;
        [SerializeField] private float scaleOnClose;
        [SerializeField] private float scaleDuration;
        
        [Header("Chat UI")] 
        [SerializeField] private TMP_Text currentChatName;
        [SerializeField] private Image currentChatAvatar;
        [SerializeField] private TMP_Text currentChatStatus;
        [SerializeField] private Image notificationIcon;
        
        private TMP_Text notificationCounterText;
        private int notificationCounter;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        private Vector3 initPos;
        
        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }

        private void Start()
        {
            openIcon.onClick.AddListener(OpenApp);
            closeButton.onClick.AddListener(CloseApp);

            initPos = gameObject.transform.localPosition;
            gameObject.transform.localScale = new Vector3(scaleOnClose, scaleOnClose, scaleOnClose);
            gameObject.SetActive(false);
            
            AskaChat.OnChatChanged += ChangeChat;
            AskaMessageSender.OnNewMessageSend += SetNotification;
            GameManager.OnGameEnd += CloseApp;

            notificationIcon.gameObject.SetActive(false);
            notificationCounterText = notificationIcon.GetComponentInChildren<TMP_Text>();
            notificationCounterText.text = "0";
            
            Chats[0].OpenChat();
            CurrentOpenedChat = Chats[0];
        }

        private void OnDestroy()
        {
            AskaChat.OnChatChanged -= ChangeChat;
            AskaMessageSender.OnNewMessageSend -= SetNotification;
            GameManager.OnGameEnd -= CloseApp;
        }

        public void ChangeChat(AskaChat chat)
        {
            currentChatName.text = chat.User.Name;
            currentChatStatus.text = chat.User.Status;
            currentChatAvatar.sprite = chat.User.ProfilePicture;

            CurrentOpenedChat = chat;
        }

        private void SetNotification(AskaChat _)
        {
            audioManager.PlayOneShot(FMODEvents.icqMessageSound);
            if (IsOpen)
            {
                ResetNotification();
            }
            else
            {
                notificationIcon.gameObject.SetActive(true);
                notificationCounter++;
                notificationCounterText.text = notificationCounter.ToString();
            }
        }

        private void ResetNotification()
        {
            notificationCounter = 0;
            notificationCounterText.text = notificationCounter.ToString();
            notificationIcon.gameObject.SetActive(false);
        }
        
        public void OpenApp()
        {
            if (IsOpen)
            {
                CloseApp();
            }
            else
            {
                ResetNotification();
                OpenAppAsync().Forget();
            }
        }

        public void CloseApp()
        {
            if (!IsOpen)
            {
                return;
            }
            CloseAppAsync().Forget();
        }

        private async UniTask OpenAppAsync()
        {
            openIcon.interactable = false;
            closeButton.interactable = false;
            
            gameObject.SetActive(true);
            await gameObject.transform.DOScale(1, scaleDuration).ToUniTask();
            
            openIcon.interactable = true;
            closeButton.interactable = true;
            IsOpen = true;
        }
        private async UniTask CloseAppAsync()
        {
            openIcon.interactable = false;
            closeButton.interactable = false;
            
            await gameObject.transform.DOScale(scaleOnClose, scaleDuration).ToUniTask();
            gameObject.SetActive(false);
            
            openIcon.interactable = true;
            closeButton.interactable = true;
            IsOpen = false;
            gameObject.transform.localPosition = initPos;
        }
    }
}