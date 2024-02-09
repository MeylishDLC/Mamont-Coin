using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Main")] 
    public GameObject interactionOff;
    public int messageDelayMilliseconds;

    [Header("Clicker")] 
    [SerializeField] private GameObject clickerCanvas;
    [SerializeField] private GameObject shopPanelCanvas;
    public static int Clicks;
    public static int Multiplier;

    [Header("Apps")] 
    [SerializeField] private GameObject skypeObject;
    private SkypeApp skypeApp;
    [SerializeField] private GameObject notepadObject;
    private NotepadInteractable notepadInteractable;


    [Header("Introduction")] 
    [SerializeField] private GameObject clickerEXE;
    [SerializeField] private List<string> scammerBeginningMessages;

    [Header("Ending")] 
    [SerializeField] private GameObject bankCardForm;
    [SerializeField] private float bankCardFormScale;
    
    [SerializeField] private List<string> scammerEndMessages;
    [SerializeField] private GameObject popupWindowsContainer;
    
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
        clickerEXE.SetActive(false);

        notepadInteractable = notepadObject.GetComponent<NotepadInteractable>();
        skypeApp = skypeObject.GetComponent<SkypeApp>();
    }

    public void GameStart()
    {
        bankCardForm.SetActive(false);
        clickerCanvas.SetActive(false);
        shopPanelCanvas.SetActive(false); 
        interactionOff.SetActive(true);

        OnSkypeOpenFirstTimeAsync().Forget();
    }
    

    public void GameEnd()
    {
        GameEndAsync().Forget();
    }
    private async UniTask GameEndAsync()
    {
        DisableAllBackgroundProcesses();
        
        clickerCanvas.SetActive(false);
        shopPanelCanvas.SetActive(false);
        
        interactionOff.SetActive(true);
        
        //reopen notepad and skype
        notepadInteractable.CloseApp();
        skypeApp.CloseApp();

        notepadObject.transform.localPosition = notepadSetPosition;
        skypeObject.transform.localPosition = skypeSetPosition;
        await UniTask.Delay(messageDelayMilliseconds);
        
        notepadInteractable.OpenApp();
        skypeApp.OpenApp();
        
        //clear all windows
        Destroy(popupWindowsContainer);
        
        ChatManager.GetInstance().SwitchToScammer();
        

        foreach (var message in scammerEndMessages)
        {
            ChatManager.GetInstance().SendMessageToScammerChat(message);
            await UniTask.Delay(messageDelayMilliseconds);
        }
        bankCardForm.SetActive(true);
        await bankCardForm.transform.DOScale(bankCardFormScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
    }
    private async UniTask OnSkypeOpenFirstTimeAsync()
    {
        await UniTask.Delay(2000);
        skypeApp.OpenApp();
        
        foreach (var message in scammerBeginningMessages)
        {
            ChatManager.GetInstance().SendMessageToScammerChat(message);
            await UniTask.Delay(messageDelayMilliseconds);
        }
        ChatManager.GetInstance().SendMessageToScammerChatWithName("<color=white>.</color>\n\n\n\n<color=white>.</color>", "exe.message");
        clickerEXE.SetActive(true);
    }
    
    
    public void OpenClicker()
    {
        clickerCanvas.SetActive(true);
        shopPanelCanvas.SetActive(true);

        interactionOff.SetActive(false);
        clickerEXE.SetActive(false);
        TaskBackgroundManager.GetInstance().trojanWarningsActive = true;
        TaskBackgroundManager.GetInstance().TrojanWarningAppear();

        Destroy(GameObject.Find("exe.message"));
    }
    
    private void DisableAllBackgroundProcesses()
    {
        TaskBackgroundManager.GetInstance().trojanWarningsActive = false;
        BoostsManager.GetInstance().autoPopupWindowEnabled = false;
        BoostsManager.GetInstance().autoClickerEnabled = false;
    }
    
}
