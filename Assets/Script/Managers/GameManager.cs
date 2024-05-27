using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.ChatScript;
using Script.Apps.ChatScript.Skamp;
using Script.Apps.NotePadScript;
using Script.Core.Popups;
using Script.Core.Popups.Spawns;
using Script.Managers.Senders;
using Script.Sound;
using Script.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Script.Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Main")] public bool Debugging;
        [SerializeField] private GameObject interactionOff;

        [Header("Apps")] 
        [SerializeField] private GameObject clickerObject;
        [SerializeField] private GameObject shopPanelObject;
        [SerializeField] private float scaleOnOpenClicker;
    
        [SerializeField] private SkypeApp skypeApp; 
        [SerializeField] private NotepadInteractable notepadInteractive;

        [Header("Introduction")] 
        [SerializeField] private RandomSpawner popupSpawner;
        [SerializeField] private UnityEvent beginningDialogueSequence;
        [SerializeField] private GameObject clickerExeMessagePrefab;
        private Button clickerExeButton;

        [Header("Ending")] 
        [SerializeField] private GameObject bankCardForm;
        [SerializeField] private float bankCardFormScale;
        [SerializeField] private UnityEvent endingDialogueSequence;

        [SerializeField] private Vector3 skypeSetPosition;
        [SerializeField] private Vector3 notepadSetPosition;

        private SkampMessageSender _skampMessageSender;

        private AudioManager audioManager;
        private FMODEvents FMODEvents;

        public static event Action OnGameEnd;

        [Inject]
        public void Construct(SpecificBoostSetter specificBoostSetter, SkampMessageSender skampMessageSender, 
            AudioManager audioManager, FMODEvents fmodEvents)
        {
            this._skampMessageSender = skampMessageSender;
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        private void Start()
        {
            interactionOff.SetActive(false);
            
            if (!Debugging)
            {
                GameStart();
            }
        }

        public void GameStart()
        {
            audioManager.PlayOneShot(FMODEvents.windowsGreetingSound);
            audioManager.InitializeMusic(FMODEvents.defaultMusic);
        
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
            await UniTask.Delay(_skampMessageSender.DelayBetweenMessagesMillisecond * 4);
            audioManager.PlayOneShot(FMODEvents.skypeMessageSound);
        
            var messageObject = Instantiate(clickerExeMessagePrefab, skypeApp.scammerChatContent.transform);
            clickerExeButton = messageObject.GetComponentInChildren<Button>();
            clickerExeButton.onClick.AddListener(OpenClicker);
        }

        private async UniTask OpenClickerAsync()
        {
            popupSpawner.StartSpawn();
            audioManager.SetMusicAct(MusicAct.MAIN);
        
            clickerObject.SetActive(true);
            await clickerObject.transform.DOScale(scaleOnOpenClicker, 0.1f).SetLoops(2, LoopType.Yoyo);
            shopPanelObject.SetActive(true);

            clickerExeButton.interactable = false;
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
            OnGameEnd?.Invoke();
        
            clickerObject.SetActive(false);
            shopPanelObject.SetActive(false);
        
            interactionOff.SetActive(true);
        
            //reopen notepad and skype
            //todo: implement for all apps
            notepadInteractive.CloseApp();
            skypeApp.CloseApp();

            notepadInteractive.gameObject.transform.localPosition = notepadSetPosition;
            skypeApp.gameObject.transform.localPosition = skypeSetPosition;
            await UniTask.Delay(1500);
        
            notepadInteractive.OpenApp();
            skypeApp.OpenApp();
            
        
            _skampMessageSender.SwitchToScammer();
            endingDialogueSequence.Invoke();

            //todo: fix that shit too
            await UniTask.Delay(_skampMessageSender.DelayBetweenMessagesMillisecond * 4);
        
            bankCardForm.SetActive(true);
            await bankCardForm.transform.DOScale(bankCardFormScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }
    
    }
}
