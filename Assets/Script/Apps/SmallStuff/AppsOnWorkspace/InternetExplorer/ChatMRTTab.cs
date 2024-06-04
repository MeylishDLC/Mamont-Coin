using System;
using Cysharp.Threading.Tasks;
using Script.Apps.ChatScript.Skamp;
using TMPro;
using UnityEngine;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.InternetExplorer
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
        
        public static event Action OnImageGenerated;
        protected override void Awake()
        {
            base.Awake();
            OpenButton.gameObject.SetActive(false);
            messageField.onEndEdit.AddListener(UserSendMessage);
        }

        private void UserSendMessage(string message)
        {
            var newMessage = new Message {Text = message};
        
            var newText = Instantiate(userMessagePrefab, chatContainer);
        
            newMessage.TextObject = newText.GetComponentInChildren<TextMeshProUGUI>();
            newMessage.TextObject.text = newMessage.Text;
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
                OnImageGenerated?.Invoke();
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