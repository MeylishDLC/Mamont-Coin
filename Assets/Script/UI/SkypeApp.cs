using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkypeApp : MonoBehaviour, IWindowedApp
{
    [Header("Open/Close Window")]
    [SerializeField] private Button closeButton;
    [SerializeField] private float scaleOnOpen;
    [SerializeField] private float scaleOnClose;
    [SerializeField] private float openDuration;
    [SerializeField] private bool closedOnStart;

    [Header("Skype Icon")] 
    [SerializeField] private GameObject windowIcon;
    [SerializeField] private GameObject notificationIcon;
    private TextMeshProUGUI notificationCounterText;
    private int notificationCounter;
    public bool isOpen { get; private set; }
    
    private void Start()
    {
        if (closedOnStart)
        {
            isOpen = false;
            gameObject.SetActive(false);
            notificationIcon.SetActive(false);

            notificationCounterText = notificationIcon.GetComponentInChildren<TextMeshProUGUI>();
            notificationCounter = 0;
            notificationCounterText.text = notificationCounter.ToString();

            Events.MessageRecieved += OnNewNotificationGet;
        }
    }

    private async UniTask CloseAppAsync()
    {
        closeButton.interactable = false;
        
        await transform.DOScale(scaleOnClose, openDuration).ToUniTask();

        closeButton.interactable = true;
        gameObject.SetActive(false);
        isOpen = false;
    }

    private async UniTask OpenAppAsync()
    {
        gameObject.SetActive(true);
        closeButton.interactable = false;
        
        await transform.DOScale(scaleOnOpen, openDuration).ToUniTask();

        closeButton.interactable = true;
        
        notificationIcon.SetActive(false);
        notificationCounter = 0;
        isOpen = true;
    }

    private void OnNewNotificationGet()
    {
        if (!isOpen)
        {
            notificationCounter++;
            
            notificationIcon.SetActive(true);
            notificationIcon.transform.DOScale(1.3f, 0.1f).SetLoops(2, LoopType.Yoyo);
            
            notificationCounterText.text = notificationCounter.ToString();
        }
    }

    public void CloseApp()
    {
        CloseAppAsync().Forget();
    }

    public void OpenApp()
    {
        OpenAppAsync().Forget();
    }
    
    
}
