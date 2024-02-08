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
    [SerializeField] private GameObject chatCanvas;
    [SerializeField] private int maxMessages;
    [SerializeField] private GameObject textObject;
    [SerializeField] private TextMeshProUGUI currentChatName;

    //todo: blue color for selected bro
    [Header("Scammer ChatBox")] 
    [SerializeField] private string scammerName;
    [SerializeField] private List<Message> scammerMessageList;
    [SerializeField] private GameObject scammerChatPanel;
    [SerializeField] private Image scammerProfileToolPanel;

    [Header("Scammer ChatBox")] 
    [SerializeField] private string hackerName;
    [SerializeField] private List<Message> hackerMessageList;
    [SerializeField] private GameObject hackerChatPanel;
    [SerializeField] private Image hackerProfileToolPanel;
    private Color originalProfilesColor;

    private static ChatManager instance;
    private void Start()
    {
        scammerChatPanel.SetActive(true);
        hackerChatPanel.SetActive(false);
        currentChatName.text = scammerName;
        
        hackerMessageList = new List<Message>();
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
        if (scammerMessageList.Count >= maxMessages)
        {
            Destroy(scammerMessageList[0].textObject.gameObject);
            scammerMessageList.Remove(scammerMessageList[0]); 
        }
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, scammerChatPanel.transform);
        
        newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        scammerMessageList.Add(newMessage);
        Events.MessageRecieved?.Invoke();
    }
    public void SendMessageToHackerChat(string text)
    {
        if (hackerMessageList.Count >= maxMessages)
        {
            Destroy(hackerMessageList[0].textObject.gameObject);
            hackerMessageList.Remove(hackerMessageList[0]); 
        }
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, hackerChatPanel.transform);
        
        newMessage.textObject = newText.GetComponentInChildren<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        hackerMessageList.Add(newMessage);
        Events.MessageRecieved?.Invoke();
    }
    
    public void SendMessageToScammerChat(string text, string messageName)
    {
        if (hackerMessageList.Count >= maxMessages)
        {
            Destroy(hackerMessageList[0].textObject.gameObject);
            hackerMessageList.Remove(hackerMessageList[0]); 
        }
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, hackerChatPanel.transform);
        newText.name = messageName;
        
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        hackerMessageList.Add(newMessage);
        Events.MessageRecieved?.Invoke();
    }

    public void SwitchToHacker()
    {
        scammerChatPanel.SetActive(false);
        hackerChatPanel.SetActive(true);

        currentChatName.text = hackerName;

        hackerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 1f);
        scammerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 0f);
    }

    public void SwitchToScammer()
    {
        hackerChatPanel.SetActive(false);
        scammerChatPanel.SetActive(true);
        
        currentChatName.text = scammerName;
        
        scammerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 1f);
        hackerProfileToolPanel.color = new Color(originalProfilesColor.r, originalProfilesColor.g, originalProfilesColor.b, 0f);
    }
}
