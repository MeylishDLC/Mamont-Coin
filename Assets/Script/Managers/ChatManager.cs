using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChatManager : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private SkypeApp skypeApp;
    [SerializeField] private GameObject textObject;
    
    private Color profileColorActive;
    private Color profileColorInactive;
    
    private bool scammerChatActive = true;
    private bool ScammerChatActive
    {
        get => scammerChatActive;
        set
        {
            scammerChatActive = value;
            
            if (scammerChatActive)
            {
                skypeApp.hackerChat.SetActive(false);
                skypeApp.scammerNotificationIcon.SetActive(false);
                
                skypeApp.scammerChat.SetActive(true);
                skypeApp.currentChatName.text = skypeApp.scammerName;

                skypeApp.scammerProfileToolPanel.color = profileColorActive;
                skypeApp.hackerProfileToolPanel.color = profileColorInactive;
            }
            else
            {
                skypeApp.scammerChat.SetActive(false);

                skypeApp.hackerNotificationIcon.SetActive(false);
                skypeApp.hackerChat.SetActive(true);

                skypeApp.currentChatName.text = skypeApp.hackerName;

                skypeApp.hackerProfileToolPanel.color = profileColorActive;
                skypeApp.scammerProfileToolPanel.color = profileColorInactive;
            }
        }
    }

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
    }
    
    public void SwitchToScammer()
    {
        ScammerChatActive = true;
    }
}
