using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript.Skamp;
using Script.Data;
using Script.Data.Dialogues;
using TMPro;
using UnityEngine;

namespace Script.Managers.Senders
{
    public class SkampMessageSender : MessageSender
    { 
        [SerializeField] private SkypeApp skypeApp;
    
        private Color profileColorActive;
        private Color profileColorInactive;
        private bool ScammerChatActive { get; set; } = true;
        public static event Action MessageRecieved;
        private void Start()
        {
            skypeApp.scammerChat.SetActive(true);
            skypeApp.hackerChat.SetActive(false);
            skypeApp.currentChatName.text = skypeApp.scammerName;

            var origColor = skypeApp.hackerProfileToolPanel.color;
            profileColorActive = new Color(origColor.r, origColor.g, origColor.b, 1f);
            profileColorInactive = new Color(origColor.r, origColor.g, origColor.b, 0f);

            skypeApp.scammerProfileToolPanel.color = profileColorActive;
        }
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
        
        public void StartDialogueSequence(string dialogueKey, SerializedDictionary<string,DialogueSpeakerPair> dialogues)
        {
            if (!dialogues.ContainsKey(dialogueKey))
            {
                Debug.LogWarning($"Could not find a dialogue key: {dialogueKey}");
                return;
            }
            var pair = dialogues[dialogueKey];
            StartDialogueSequenceAsync(pair.chatCharacter, pair.dialogueLines).Forget();
        }

        private void SendNewMessage(string message, Transform chatContainer)
        {
            var newMessage = new Message {Text = message};
        
            var newText = Instantiate(TextObject, chatContainer);
        
            newMessage.TextObject = newText.GetComponentInChildren<TextMeshProUGUI>();
            newMessage.TextObject.text = newMessage.Text;
        
            newText.transform.DOScale(skypeApp.messageScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        
            MessageRecieved?.Invoke();
        }

        private async UniTask StartDialogueSequenceAsync(ChatCharacter chatCharacter, List<string> dialogueLines)
        {
            foreach (var line in dialogueLines)
            {
                if (chatCharacter is ChatCharacter.Hacker)
                {
                    SendMessageToHackerChat(line);
                }
                else
                {
                    SendMessageToScammerChat(line);
                }
                await UniTask.Delay(DelayBetweenMessagesMillisecond);
            }
        }
        
        public void SendMessageToScammerChat(string text)
        {
            if (!ScammerChatActive)
            {
                skypeApp.scammerNotificationIcon.SetActive(true);
            }
            
            SendNewMessage(text, skypeApp.scammerChatContent.transform);
        }
        public void SendMessageToHackerChat(string text)
        {
            if (ScammerChatActive)
            {
                skypeApp.hackerNotificationIcon.SetActive(true);
            }

            SendNewMessage(text, skypeApp.hackerChatContent.transform);
        }

        public void SwitchToHacker()
        {
            ScammerChatActive = false;
            skypeApp.scammerChat.SetActive(false);

            skypeApp.hackerNotificationIcon.SetActive(false);
            skypeApp.hackerChat.SetActive(true);

            skypeApp.currentChatName.text = skypeApp.hackerName;

            skypeApp.hackerProfileToolPanel.color = profileColorActive;
            skypeApp.scammerProfileToolPanel.color = profileColorInactive;
        }
    
        public void SwitchToScammer()
        {
            ScammerChatActive = true;
            skypeApp.hackerChat.SetActive(false);
            skypeApp.scammerNotificationIcon.SetActive(false);
                
            skypeApp.scammerChat.SetActive(true);
            skypeApp.currentChatName.text = skypeApp.scammerName;

            skypeApp.scammerProfileToolPanel.color = profileColorActive;
            skypeApp.hackerProfileToolPanel.color = profileColorInactive;
        }
    }
}
