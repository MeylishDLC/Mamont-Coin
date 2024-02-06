using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    [Header("ChatBox")] [SerializeField] private int maxMessages;
    [SerializeField] private List<Message> messageList;
    [SerializeField] private GameObject chatPanel;
    [SerializeField] private GameObject textObject;
    [SerializeField] private GameObject chatCanvas;

    private static ChatManager instance;
    private void Start()
    {
        messageList = new List<Message>();
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

    private void Update()
    {
        if (InputManager.GetInstance().GetKeyboardButtonPressed())
        {
            SendMessageToChat("Keyboard button was pressed");
        }
    }
    
    public void SendMessageToChat(string text)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]); 
        }
        var newMessage = new Message {text = text};
        
        var newText = Instantiate(textObject, chatPanel.transform);
        
        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();
        newMessage.textObject.text = newMessage.text;
        
        messageList.Add(newMessage);
        Events.MessageRecieved?.Invoke();
    }
}
