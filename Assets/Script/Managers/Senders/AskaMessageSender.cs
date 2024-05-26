using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript;
using Script.Apps.ChatScript.Aska;
using Script.Data;
using TMPro;
using UnityEngine;

namespace Script.Managers
{
    public class AskaMessageSender: MessageSender
    {
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

        private void SendNewMessage(string message, AskaChat chat)
        {
            var newMessage = new Message {text = message};
        
            var newText = Instantiate(TextObject, chat.MessageContainer.transform);
        
            newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
            newMessage.textObject.text = newMessage.text;
        
            newText.transform.DOScale(aska.MessageScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        
            OnNewMessageSend?.Invoke(chat);
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
    }
}