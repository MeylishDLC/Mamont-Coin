using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Apps.Duralingo;
using Script.Data.Dialogues;
using Script.Managers.Senders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Core.RandomEvents
{
    [System.Serializable]
    public class ChineseTutorEvent
    {
        [SerializeField] private Button replyButton;
        [SerializeField] private string firstReply;
        [SerializeField] private string secondReply;
        [SerializeField] private SerializedDictionary<string, DialogueSpeakerPair> askaDialogues;
        [SerializeField] private DuralingoGame duralingoGame;
        
        [Header("Messages Prefabs")]
        [SerializeField] private TMP_Text playerMessagePrefab;
        [SerializeField] private Transform messageContainer;
        [SerializeField] private int delayBeforeAnswerMilliseconds;

        private bool answered;
        private TMP_Text replyText;
        private AskaMessageSender askaSender;
        
        public void Construct(AskaMessageSender askaMessageSender)
        {
            askaSender = askaMessageSender;

            replyText = replyButton.GetComponentInChildren<TMP_Text>();
            replyText.text = firstReply;
            replyButton.onClick.AddListener(OnAnswerSend);
            replyButton.gameObject.SetActive(false);
        }

        public void StartEvent()
        {
            StartEventAsync().Forget();
        }
        
        private async UniTask StartEventAsync()
        {
            var pair = askaDialogues[askaDialogues.Keys.ElementAt(0)];
            await askaSender.StartDialogueSequenceAsync(pair.chatCharacter, pair.dialogueLines);
            replyButton.gameObject.SetActive(true);
        }

        private void OnAnswerSend()
        {
            if (!answered)
            {
                OnAnswerSendAsync(firstReply, 1).Forget();
                replyText.text = secondReply;
            }
            else
            {
                OnAnswerSendAsync(secondReply, 2).Forget();
            }
        }

        private async UniTask OnAnswerSendAsync(string reply, int dialogueIndex)
        {
            replyButton.gameObject.SetActive(false);
            
            var plrMessage = Object.Instantiate(playerMessagePrefab, messageContainer);
            plrMessage.text = reply;
            await UniTask.Delay(delayBeforeAnswerMilliseconds);
            
            var pair = askaDialogues[askaDialogues.Keys.ElementAt(dialogueIndex)];
            await askaSender.StartDialogueSequenceAsync(pair.chatCharacter, pair.dialogueLines);
            await UniTask.Delay(delayBeforeAnswerMilliseconds);

            if (answered)
            {
                duralingoGame.OpenApp();
            }
            else
            {
                replyButton.gameObject.SetActive(true);
                answered = true;
            }
        }
        
    }
}