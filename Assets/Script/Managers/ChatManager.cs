using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
    [SerializeField] private TextMeshProUGUI currentChatName;
    
    [Header("Scammer ChatBox")] 
    [SerializeField] private string scammerName;
    [SerializeField] private GameObject scammerChat; 
    [SerializeField] private GameObject scammerChatContent;
    [SerializeField] private Image scammerProfileToolPanel;
    [SerializeField] private GameObject scammerNotificationIcon;

    private List<Message> scammerMessageList;

    [Header("Hacker ChatBox")] 
    [SerializeField] private string hackerName;
    [SerializeField] private GameObject hackerChat;
    [SerializeField] private GameObject hackerChatContent;
    [SerializeField] private Image hackerProfileToolPanel;
    [SerializeField] private GameObject hackerNotificationIcon;
    
    private List<Message> hackerMessageList;
    
    private Color originalProfilesColor;
    private bool scammerChatActive = true;

    private static ChatManager instance;
    private void Start()
    {
        scammerChat.SetActive(true);
        hackerChat.SetActive(false);
        currentChatName.text = scammerName;
        
        hackerMessageList = new List<Message>();
        scammerMessageList = new List<Message>();
        
        originalProfilesColor = hackerProfileToolPanel.color;
        scammerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 1f);
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Chat Manager in the scene.");
        }
        instance = this;
    }

    public static ChatManager GetInstance()
    {
        return instance;
    }
    
    public void SendMessageToScammerChat(string text)
    {
        if (!scammerChatActive)
        {
            scammerNotificationIcon.SetActive(true);
            Debug.Log("Notif");
        }
        
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, scammerChatContent.transform);
        
        newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        scammerMessageList.Add(newMessage);
        Events.MessageRecieved?.Invoke();
    }
    public void SendMessageToHackerChat(string text)
    {
        if (scammerChatActive)
        {
            hackerNotificationIcon.SetActive(true);
            Debug.Log("Notif");
        }

        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, hackerChatContent.transform);
        
        newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        hackerMessageList.Add(newMessage);
        Events.MessageRecieved?.Invoke();
    }

    public void SwitchToHacker()
    {
        scammerChatActive = false;
        scammerChat.SetActive(false);
        
        hackerNotificationIcon.SetActive(false);
        hackerChat.SetActive(true);

        currentChatName.text = hackerName;

        hackerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 1f);
        scammerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 0f);
    }

    public void SwitchToScammer()
    {
        scammerChatActive = true;
        hackerChat.SetActive(false);
        
        scammerNotificationIcon.SetActive(false);
        scammerChat.SetActive(true);
        
        currentChatName.text = scammerName;
        
        scammerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 1f);
        hackerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 0f);
    }
}
