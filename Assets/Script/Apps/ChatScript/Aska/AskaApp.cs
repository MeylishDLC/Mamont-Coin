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
        
        private bool isOpen;
        private TMP_Text notificationCounterText;
        private int notificationCounter;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        
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
            
            gameObject.SetActive(false);
            gameObject.transform.localScale = new Vector3(scaleOnClose, scaleOnClose, scaleOnClose);
            
            AskaChat.OnChatChanged += ChangeChat;
            AskaMessageSender.OnNewMessageSend += SetNotification;

            notificationIcon.gameObject.SetActive(false);
            notificationCounterText = notificationIcon.GetComponentInChildren<TMP_Text>();
            notificationCounterText.text = "0";
            
            Chats[0].OpenChat();
            CurrentOpenedChat = Chats[0];
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
            if (isOpen)
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
            ResetNotification();
            OpenAppAsync().Forget();
        }

        public void CloseApp()
        {
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
        }
        private async UniTask CloseAppAsync()
        {
            openIcon.interactable = false;
            closeButton.interactable = false;
            
            await gameObject.transform.DOScale(scaleOnClose, scaleDuration).ToUniTask();
            gameObject.SetActive(false);
            
            openIcon.interactable = true;
            closeButton.interactable = true;
        }
    }
}