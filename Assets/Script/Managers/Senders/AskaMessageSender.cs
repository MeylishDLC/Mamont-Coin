using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript.Aska;
using Script.Apps.ChatScript.Skamp;
using Script.Data;
using Script.Data.Dialogues;
using TMPro;
using UnityEngine;

namespace Script.Managers.Senders
{
    public class AskaMessageSender: MessageSender
    {
        [SerializeField] private SerializedDictionary<string, List<DialogueMessagePair>> a4ChatDialogues;
        [SerializeField] private AskaApp aska;

        public static event Action<AskaChat> OnNewMessageSend;
       
        public override void StartDialogueSequence(string dialogueKey)
        {
            if (!Dialogues.ContainsKey(dialogueKey))
            {
                Debug.LogWarning($"Could not find a dialogue key: {dialogueKey}");
                return;
            }
            var pair = Dialogues[dialogueKey];
            StartDialogueSequenceAsync(pair.chatCharacter, pair.dialogueLines).Forget();
        }
        
        public void StartDialogueSequence(string dialogueKey, SerializedDictionary<string, DialogueSpeakerPair> dialogues)
        {
            if (!dialogues.ContainsKey(dialogueKey))
            {
                Debug.LogWarning($"Could not find a dialogue key: {dialogueKey}");
                return;
            }
            var pair = dialogues[dialogueKey];
            StartDialogueSequenceAsync(pair.chatCharacter, pair.dialogueLines).Forget();
        }
           
        public void StartA4ChatDialogueSequence(string dialogueKey)
        {
            if (!a4ChatDialogues.ContainsKey(dialogueKey))
            {
                Debug.LogWarning($"Could not find a dialogue key: {dialogueKey}.");
                return;
            }

            var dialogue = a4ChatDialogues[dialogueKey];
            StartDialogueSequenceAsync(dialogue).Forget();
        }
        private async UniTask StartDialogueSequenceAsync(ChatCharacter chatCharacter, List<string> dialogueLines)
        {
            var dialogueChat = aska.Chats
                .FirstOrDefault(chat => chat.User.CharacterType == chatCharacter);
            
            foreach (var line in dialogueLines)
            {
                SendNewMessage(line,dialogueChat);
                await UniTask.Delay(DelayBetweenMessagesMillisecond);
            }
        }
        
        private async UniTask StartDialogueSequenceAsync(List<DialogueMessagePair> dialogue)
        {
            var dialogueChat = aska.Chats
                .FirstOrDefault(chat => chat.User.CharacterType == ChatCharacter.A4);
            

            foreach (var message in dialogue)
            {
                SendNewMessage(message.DialogueLine, dialogueChat, message.SpeakerMessagePrefab);
                await UniTask.Delay(DelayBetweenMessagesMillisecond);
            }
        }
        
        private void SendNewMessage(string message, AskaChat chat)
        {
            var newMessage = new Message {text = message};
        
            var newText = Instantiate(TextObject, chat.MessageContainer.transform);
        
            newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
            newMessage.textObject.text = newMessage.text;
        
            newText.transform.DOScale(aska.MessageScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        
            OnNewMessageSend?.Invoke(chat);
        }
        private void SendNewMessage(string message, AskaChat chat, GameObject messagePrefab)
        {
            var newMessage = new Message {text = message};
        
            var newText = Instantiate(messagePrefab, chat.MessageContainer.transform);


            newMessage.textObject = newText.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
            newMessage.textObject.text = newMessage.text;
        
            newText.transform.DOScale(aska.MessageScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        
            OnNewMessageSend?.Invoke(chat);
        }
        
        [Serializable]
        private class DialogueMessagePair
        {
            [field: SerializeField] public GameObject SpeakerMessagePrefab { get; private set; }
          
            [TextArea]
            [field: SerializeField] public string DialogueLine { get; private set; }
        }
    }
    
    
}