using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Sound;
using Script.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static int Clicks;
    public static int Multiplier;
    
    [Header("Main")] 
    public GameObject interactionOff;
    public int messageDelayMilliseconds;

    [Header("Apps")] 
    [SerializeField] private GameObject clickerObject;
    [SerializeField] private GameObject shopPanelObject;
    [SerializeField] private float scaleOnOpenClicker;
    
    [SerializeField] private SkypeApp skypeApp;
    [SerializeField] private NotepadInteractable notepadInteractable;
    
    [Header("Introduction")] 
    [SerializeField] private GameObject clickerExeMessagePrefab;
    [SerializeField] private List<string> scammerBeginningMessages;
    private Button clickerExeButton;

    [Header("Ending")] 
    [SerializeField] private GameObject bankCardForm;
    [SerializeField] private float bankCardFormScale;
    
    [SerializeField] private List<string> scammerEndMessages;

    [SerializeField] private Vector3 skypeSetPosition;
    [SerializeField] private Vector3 notepadSetPosition;
    
    private static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one GameManager in the scene.");
        }
        instance = this;
    }

    public static GameManager GetInstance()
    {
        return instance;
    }
    
    private void Start()
    {
        ///////////////
        Clicks = 0;
        Multiplier = 1;
        //////////////

        interactionOff.SetActive(false);
    }

    public void GameStart()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.windowsGreetingSound);
        AudioManager.instance.InitializeMusic(FMODEvents.instance.defaultMusic);
        
        bankCardForm.SetActive(false);
        
        clickerObject.SetActive(false);
        shopPanelObject.SetActive(false);

        OnSkypeOpenFirstTimeAsync().Forget();
    }
    private async UniTask OnSkypeOpenFirstTimeAsync()
    {
        await UniTask.Delay(2000);

        foreach (var message in scammerBeginningMessages)
        {
            ChatManager.GetInstance().SendMessageToScammerChat(message);
            await UniTask.Delay(messageDelayMilliseconds);
        }

        var messageObject = Instantiate(clickerExeMessagePrefab, skypeApp.scammerChatContent.transform);
        clickerExeButton = messageObject.GetComponentInChildren<Button>();
        clickerExeButton.onClick.AddListener(OpenClicker);
    }

    private async UniTask OpenClickerAsync()
    {
        AudioManager.instance.SetMusicAct(MusicAct.MAIN);
        
        clickerObject.SetActive(true);
        await clickerObject.transform.DOScale(scaleOnOpenClicker, 0.1f).SetLoops(2, LoopType.Yoyo);
        shopPanelObject.SetActive(true);

        clickerExeButton.interactable = false;
        PopupsManager.GetInstance().TrojanWarningAppear();
    }
    private void OpenClicker()
    {
        clickerExeButton.onClick.RemoveListener(OpenClicker);
        OpenClickerAsync().Forget();
    }
    
    public void GameEnd()
    {
        GameEndAsync().Forget();
    }
    
    private async UniTask GameEndAsync()
    {
        DisableAllBackgroundProcesses();
        
        clickerObject.SetActive(false);
        shopPanelObject.SetActive(false);
        
        interactionOff.SetActive(true);
        
        //reopen notepad and skype
        notepadInteractable.CloseApp();
        skypeApp.CloseApp();

        notepadInteractable.gameObject.transform.localPosition = notepadSetPosition;
        skypeApp.gameObject.transform.localPosition = skypeSetPosition;
        await UniTask.Delay(messageDelayMilliseconds);
        
        notepadInteractable.OpenApp();
        skypeApp.OpenApp();
        
        //clear all windows
        Destroy(PopupsManager.GetInstance().PopupsContainer);
        
        ChatManager.GetInstance().SwitchToScammer();

        foreach (var message in scammerEndMessages)
        {
            ChatManager.GetInstance().SendMessageToScammerChat(message);
            await UniTask.Delay(messageDelayMilliseconds);
        }
        bankCardForm.SetActive(true);
        await bankCardForm.transform.DOScale(bankCardFormScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
    }
    
    private void DisableAllBackgroundProcesses()
    {
        PopupsManager.GetInstance().DisableAllPopups();
        BoostsManager.GetInstance().DisableAllBoosts();
    }
    
}
