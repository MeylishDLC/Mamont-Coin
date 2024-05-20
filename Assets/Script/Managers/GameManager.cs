using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript;
using Script.Apps.NotePadScript;
using Script.Sound;
using Script.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Script.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Main")] public bool Debugging;
        public GameObject interactionOff;

        [Header("Apps")] 
        [SerializeField] private GameObject clickerObject;
        [SerializeField] private GameObject shopPanelObject;
        [SerializeField] private float scaleOnOpenClicker;
    
        [SerializeField] private SkypeApp skypeApp; 
        [SerializeField] private NotepadInteractable notepadInteractive;
        [SerializeField] private NotepadWithChoices notepadWithChoices;

        [Header("Introduction")] 
        [SerializeField] private UnityEvent beginningDialogueSequence;
        [SerializeField] private GameObject clickerExeMessagePrefab;
        private Button clickerExeButton;

        [Header("Ending")] 
        [SerializeField] private GameObject bankCardForm;
        [SerializeField] private float bankCardFormScale;
        [SerializeField] private UnityEvent endingDialogueSequence;

        [SerializeField] private Vector3 skypeSetPosition;
        [SerializeField] private Vector3 notepadSetPosition;

        private BoostsManager boostsManager;
        private PopupsManager popupsManager;
    
        #region Set Instance

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

        #endregion
        private void Start()
        {
            boostsManager = notepadWithChoices.BoostsManager;
            popupsManager = PopupsManager.Instance;
            
            interactionOff.SetActive(false);
            
            if (!Debugging)
            {
                GameStart();
            }
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
        
            beginningDialogueSequence.Invoke();
        
            //todo: fix that shit
            await UniTask.Delay(ChatManager.instance.delayBetweenMessagesMillisecond * 4);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.skypeMessageSound);
        
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
            //activate trojan warnings
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

        //todo: finish
        public void CloseAllApps(IEnumerable<IWindowedApp> windowedApps)
        {
            foreach (var app in windowedApps)
            {
                app.CloseApp();
            }
        }
        private async UniTask GameEndAsync()
        {
            DisableAllBackgroundProcesses();
        
            clickerObject.SetActive(false);
            shopPanelObject.SetActive(false);
        
            interactionOff.SetActive(true);
        
            //reopen notepad and skype
            notepadInteractive.CloseApp();
            skypeApp.CloseApp();

            notepadInteractive.gameObject.transform.localPosition = notepadSetPosition;
            skypeApp.gameObject.transform.localPosition = skypeSetPosition;
            await UniTask.Delay(1500);
        
            notepadInteractive.OpenApp();
            skypeApp.OpenApp();
        
            //clear all windows
            popupsManager.DisableAllPopups(true);
        
            ChatManager.instance.SwitchToScammer();
            endingDialogueSequence.Invoke();

            //todo: fix that shit too
            await UniTask.Delay(ChatManager.instance.delayBetweenMessagesMillisecond * 4);
        
            bankCardForm.SetActive(true);
            await bankCardForm.transform.DOScale(bankCardFormScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }
    
        private void DisableAllBackgroundProcesses()
        {
            popupsManager.DisableAllPopups();
            boostsManager.DisableAllBoosts();
        }
    
    }
}
