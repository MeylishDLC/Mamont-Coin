﻿using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Script.Apps.InternetExplorer;
using Script.Data.Dialogues;
using Script.Managers.Senders;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;
using Object = UnityEngine.Object;

namespace Script.Core.RandomEvents
{
    [System.Serializable]
    public class ArtClubEvent
    {
        [SerializeField] private ChatMRTTab chatMRTTab;
        [SerializeField] private GameObject messageWithImagePrefab;
        [SerializeField] private Transform messageContainer;
        
        [SerializeField] private Button sendImageButton;
        
        [SerializeField] private SerializedDictionary<string, DialogueSpeakerPair> askaDialogues; 
        [SerializeField] private SerializedDictionary<string, DialogueSpeakerPair> skampDialogues;

        [Header("Delays")] 
        [SerializeField] private int delayBetweenMomAndHackerMilliseconds; 
        [SerializeField] private int delayBeforeMomAnswerMilliseconds;
        
        private AskaMessageSender askaSender;
        private SkampMessageSender skampSender;
        
        public static Action OnImageGenerated;
        public void Construct(AskaMessageSender askaMessageSender, SkampMessageSender skampMessageSender)
        {
            askaSender = askaMessageSender;
            skampSender = skampMessageSender;
            
            OnImageGenerated += UpdateEvent;
            sendImageButton.onClick.AddListener(OnImageSend);
        }

        public void StartEvent()
        {
            StartEventAsync().Forget();
        }
        private async UniTask StartEventAsync()
        {
            askaSender.StartDialogueSequence(askaDialogues.Keys.ToList()[0], askaDialogues);
            await UniTask.Delay(delayBetweenMomAndHackerMilliseconds);
            skampSender.StartDialogueSequence(askaDialogues.Keys.ToList()[0], skampDialogues);
            chatMRTTab.AddFreeRequestToMRT();
        }

        private void UpdateEvent()
        {
            sendImageButton.gameObject.SetActive(true);
            skampSender.StartDialogueSequence(askaDialogues.Keys.ToList()[1], skampDialogues);
        }

        private void OnImageSend()
        {
            OnImageSendAsync().Forget();
        }

        private async UniTask OnImageSendAsync()
        {
            sendImageButton.gameObject.SetActive(false);
            
            Object.Instantiate(messageWithImagePrefab, messageContainer);
            await UniTask.Delay(delayBeforeMomAnswerMilliseconds);
            askaSender.StartDialogueSequence(askaDialogues.Keys.ToList()[1], askaDialogues);
        }
    }
}