using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject interactionOff;
    [Header("Clicker")] 
    [SerializeField] private GameObject clickerCanvas;
    [SerializeField] private GameObject shopPanelCanvas;
    public static int Clicks;
    public static int Multiplier;

    [Header("Skype")] 
    [SerializeField] private GameObject skypeObject;
    [SerializeField] private Button skypeCloseButton;
    [SerializeField] private GameObject skypeNotification;
    private SkypeApp skypeApp;

    [Header("Introduction")] 
    [SerializeField] private GameObject clickerEXE;
    [SerializeField] private List<string> hackerMessages;
    
    [Header("DEBUG")]
    [SerializeField] private bool gameIntroductionEnabled;

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
        
        if (gameIntroductionEnabled)
        {
            skypeApp = skypeObject.GetComponent<SkypeApp>();
            skypeObject.GetComponent<DragnDrop>().enabled = false;
            skypeCloseButton.interactable = false;

            clickerCanvas.SetActive(false);
            shopPanelCanvas.SetActive(false); 
            interactionOff.SetActive(true);

            OnSkypeOpenFirstTimeAsync().Forget();
        }
    }
    private async UniTask OnSkypeOpenFirstTimeAsync()
    {
        await UniTask.Delay(2000);
        skypeApp.OpenApp();
        
        foreach (var message in hackerMessages)
        {
            ChatManager.GetInstance().SendMessageToScammerChat(message);
            await UniTask.Delay(1500);
        }
        ChatManager.GetInstance().SendMessageToScammerChatWithName("<color=white>.</color>\n\n\n\n<color=white>.</color>", "exe.message");
        clickerEXE.SetActive(true);
    }

    
    public void OpenClicker()
    {
        skypeCloseButton.interactable = true;
        clickerCanvas.SetActive(true);
        shopPanelCanvas.SetActive(true);
        
        interactionOff.SetActive(false);
        clickerEXE.SetActive(false);
        
        Destroy(GameObject.Find("exe.message"));
        skypeObject.GetComponent<DragnDrop>().enabled = true;
  
    }
}
