using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Apps.SmallStuff.AppsOnWorkspace.AnapaApp;
using Script.Data.Dialogues;
using Script.Managers.Senders;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Core.RandomEvents
{
    [System.Serializable]
    public class AnapaEvent
    {
        [SerializeField] private Button replyButton;
        [SerializeField] private string firstReply;
        [SerializeField] private string secondReply;
        [SerializeField] private SerializedDictionary<string, DialogueSpeakerPair> askaDialogues;
        [SerializeField] private AnapaApp anapaApp;
        
        [Header("Messages Prefabs")]
        [SerializeField] private TMP_Text playerMessagePrefab;
        [SerializeField] private GameObject playerMessageWithImagePrefab;
        [SerializeField] private Transform messageContainer;
        [SerializeField] private int delayBeforeAnswerMilliseconds;

        private bool canSend;
        private TMP_Text replyText;
        private AskaMessageSender askaSender;
        private Photo currentChosenPhoto;
        
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

        private void OnEventEnd()
        {
            foreach (var photo in anapaApp.photos)
            {
                photo.OnPhotoClicked -= ChoosePhoto;
            }
        }
        private void ChoosePhoto(Photo photo)
        {
            if (!canSend)
            {
                canSend = true;
                replyText.text = secondReply;
                replyButton.gameObject.SetActive(true);
            }
            currentChosenPhoto = photo;
        }
        private void OnAnswerSend()
        {
            if (canSend)
            {
                SendPhoto().Forget();
            }
            else
            {
                Answer().Forget();
                foreach (var photo in anapaApp.photos)
                {
                    photo.OnPhotoClicked += ChoosePhoto;
                }
            }
        }
        private async UniTask Answer()
        {
            replyButton.gameObject.SetActive(false);
            replyText.text = secondReply;
            
            var plrMessage = Object.Instantiate(playerMessagePrefab, messageContainer);
            plrMessage.text = firstReply;
            await UniTask.Delay(delayBeforeAnswerMilliseconds);
            
            var pair = askaDialogues[askaDialogues.Keys.ElementAt(1)];
            await askaSender.StartDialogueSequenceAsync(pair.chatCharacter, pair.dialogueLines);
            await UniTask.Delay(delayBeforeAnswerMilliseconds);
        }
        private async UniTask SendPhoto()
        {
            replyButton.gameObject.SetActive(false);
            
            var message = Object.Instantiate(playerMessageWithImagePrefab, messageContainer);
            var messagePic = message.GetComponentInChildren<Image>(); 
            messagePic.sprite = currentChosenPhoto.Picture;
            await UniTask.Delay(delayBeforeAnswerMilliseconds);
            askaSender.StartDialogueSequence(askaDialogues.Keys.ElementAt(2), askaDialogues);
            
            OnEventEnd();
        }
    }
}