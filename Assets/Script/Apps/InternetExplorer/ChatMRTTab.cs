using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript.Skamp;
using Script.Core.RandomEvents;
using TMPro;
using UnityEngine;

namespace Script.Apps.InternetExplorer
{
    public class ChatMRTTab: Tab, ISearchable
    {
        [field:SerializeField] public string[] SearchVariations { get; set; }
        [SerializeField] private TMP_InputField messageField;
        [SerializeField] private TMP_Text requestsText;
        
        [Header("Message Prefabs")]
        [SerializeField] private Transform chatContainer;
        [SerializeField] private GameObject userMessagePrefab;
        [SerializeField] private GameObject chatMRTDefaultMessagePrefab;
        [SerializeField] private GameObject chatMRTMessageWithPicturePrefab;
        [SerializeField] private int MRTAnswerDelayMilliseconds;
        private bool hasRequests;
        protected override void Awake()
        {
            base.Awake();
            OpenButton.gameObject.SetActive(false);
            messageField.onEndEdit.AddListener(UserSendMessage);
        }

        private void UserSendMessage(string message)
        {
            var newMessage = new Message {text = message};
        
            var newText = Instantiate(userMessagePrefab, chatContainer);
        
            newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
            newMessage.textObject.text = newMessage.text;
            MRTSendMessageAsync().Forget();
        }

        private async UniTask MRTSendMessageAsync()
        {
            await UniTask.Delay(MRTAnswerDelayMilliseconds);
            if (hasRequests)
            {
                Instantiate(chatMRTMessageWithPicturePrefab, chatContainer);
                requestsText.text = "Осталось басплатных запросов: 0";
                hasRequests = false;
                ArtClubEvent.OnImageGenerated?.Invoke();
            }
            else
            {
                Instantiate(chatMRTDefaultMessagePrefab, chatContainer);
            }
        }
        public void AddFreeRequestToMRT()
        {
            requestsText.text = "Осталось басплатных запросов: 1";
            hasRequests = true;
        }
        public void ActionOnSearch()
        {
            OpenButton.gameObject.SetActive(true);
            OpenTab();
        }
    }
}