using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using Script.Data;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private SkypeApp skypeApp;
    [SerializeField] private GameObject textObject;

    [Header("Dialogues")] 
    [SerializeField] private SerializedDictionary<string, DialogueSpeakerPair> dialogues; 
    [SerializeField] public int delayBetweenMessagesMillisecond;
    
    private Color profileColorActive;
    private Color profileColorInactive;
    private bool ScammerChatActive { get; set; } = true;

    private static ChatManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Chat Manager in the scene.");
        }
        instance = this;
        
        skypeApp.scammerChat.SetActive(true);
        skypeApp.hackerChat.SetActive(false);
        skypeApp.currentChatName.text = skypeApp.scammerName;

        var origColor = skypeApp.hackerProfileToolPanel.color;
        profileColorActive = new Color(origColor.r, origColor.g, origColor.b, 1f);
        profileColorInactive = new Color(origColor.r, origColor.g, origColor.b, 0f);

        skypeApp.scammerProfileToolPanel.color = profileColorActive;
    }

    public void StartDialogueSequence(string dialogueKey)
    {
        if (!dialogues.ContainsKey(dialogueKey))
        {
            Debug.LogWarning($"Could not find a dialogue key: {dialogueKey}");
            return;
        }
        var pair = dialogues[dialogueKey];
        StartDialogueSequenceAsync(pair.character, pair.dialogueLines).Forget();
    }

    private async UniTask StartDialogueSequenceAsync(Character character, List<string> dialogueLines)
    {
        foreach (var line in dialogueLines)
        {
            if (character is Character.Hacker)
            {
                SendMessageToHackerChat(line);
            }
            else
            {
                SendMessageToScammerChat(line);
            }
            await UniTask.Delay(delayBetweenMessagesMillisecond);
        }
    }

    public static ChatManager GetInstance()
    {
        return instance;
    }
    
    
    public void SendMessageToScammerChat(string text)
    {
        if (!ScammerChatActive)
        {
            skypeApp.scammerNotificationIcon.SetActive(true);
        }
        
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, skypeApp.scammerChatContent.transform);
        
        newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        newText.transform.DOScale(skypeApp.messageScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        
        Events.MessageRecieved?.Invoke();
    }
    public void SendMessageToHackerChat(string text)
    {
        if (ScammerChatActive)
        {
            skypeApp.hackerNotificationIcon.SetActive(true);
        }

        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, skypeApp.hackerChatContent.transform);
        
        newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;

        newText.transform.DOScale(skypeApp.messageScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        
        Events.MessageRecieved?.Invoke();
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
