using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FMOD.Studio;
using Script.Apps.ChatScript.Skamp;
using Script.Apps.NotePadScript;
using Script.Apps.SmallStuff.AppsOnWorkspace.YunixMusic;
using Script.Core.Boosts;
using Script.Core.Popups;
using Script.Core.Popups.Spawns;
using Script.Data;
using Script.Managers.Senders;
using Script.Sound;
using Script.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;
using Vector3 = UnityEngine.Vector3;

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
        [SerializeField] private NotepadWithChoices notepadWithChoices;
        [SerializeField] private YunixMusicApp yunixMusicApp; 
        
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
        
        private SkampMessageSender skampMessageSender;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        
        private List<Boost> boosts = new();
        private PopupContainer popupContainer;

        [Inject]
        public void Construct(SpecificBoostSetter specificBoostSetter, SkampMessageSender skampMessageSender, 
            AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.skampMessageSender = skampMessageSender;
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        private void Start()
        {
            boosts = notepadWithChoices.ScammerChoiceBoost.Values
                .Concat(notepadWithChoices.HackerChoiceBoost.Values)
                .ToList();
            
            popupContainer = FindAnyObjectByType<PopupContainer>();
            if (popupContainer is null)
            {
                Debug.LogError("Popup container was not found in the scene");
            }
            
            interactionOff.SetActive(false);
            if (!Debugging)
            {
                GameStart();
            }
        }

        public void GameStart()
        {
            audioManager.PlayOneShot(FMODEvents.windowsGreetingSound);
            audioManager.InitializeMusic("Default Music",FMODEvents.defaultMusic);
        
            bankCardForm.SetActive(false);
        
            clickerObject.SetActive(false);
            shopPanelObject.SetActive(false);

            OnSkypeOpenFirstTimeAsync().Forget();
        }
        private async UniTask OnSkypeOpenFirstTimeAsync()
        {
            await UniTask.Delay(2000);
        
            beginningDialogueSequence.Invoke();
        
            //todo: refactor
            await UniTask.Delay(skampMessageSender.DelayBetweenMessagesMillisecond * 4);
            audioManager.PlayOneShot(FMODEvents.skypeMessageSound);
        
            var messageObject = Instantiate(clickerExeMessagePrefab, skypeApp.scammerChatContent.transform);
            clickerExeButton = messageObject.GetComponentInChildren<Button>();
            clickerExeButton.onClick.AddListener(OpenClicker);
        }

        private async UniTask OpenClickerAsync()
        {
            popupSpawner.StartSpawn();

            if (audioManager.HasMusic("Default Music"))
            {
                audioManager.SetMusicAct(MusicAct.MAIN);
            }
            else
            {
                audioManager.StopMusic(yunixMusicApp.CurrentMusicName, STOP_MODE.IMMEDIATE);
                audioManager.InitializeMusic("Default Music", FMODEvents.defaultMusicSingle);
                yunixMusicApp.CurrentMusicName = "Default Music";
            }
        
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
            CloseAllApps();
            DisableAllBoosts();
            popupSpawner.StopSpawn();
            popupContainer.Clear();
        
            clickerObject.SetActive(false);
            shopPanelObject.SetActive(false);
        
            interactionOff.SetActive(true);

            notepadInteractive.gameObject.transform.localPosition = notepadSetPosition;
            skypeApp.gameObject.transform.localPosition = skypeSetPosition;
            await UniTask.Delay(1500);
        
            notepadInteractive.OpenApp();
            skypeApp.OpenApp();
            
        
            skampMessageSender.SwitchToScammer();
            endingDialogueSequence.Invoke();

            //todo: refactor that too
            await UniTask.Delay(skampMessageSender.DelayBetweenMessagesMillisecond * 4);
        
            bankCardForm.SetActive(true);
            await bankCardForm.transform.DOScale(bankCardFormScale, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
        }

        private void DisableAllBoosts()
        {
            foreach (var boost in boosts)
            {
                boost.Disable();
            }
        }
        private void CloseAllApps()
        {
            //todo: ??? refactor
            var sceneObjects = FindObjectsByType<MonoBehaviour>((FindObjectsSortMode) FindObjectsInactive.Exclude);

            foreach (var currentObj in sceneObjects)
            {
                var currentComponent = currentObj.GetComponent<IWindowedApp>();

                if (currentComponent != null)
                {
                    currentComponent.CloseApp();
                }
            }
        }
        
    }
}
