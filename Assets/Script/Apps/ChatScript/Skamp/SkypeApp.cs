using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Managers.Senders;
using Script.Sound;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Script.Apps.ChatScript.Skamp
{
    public class SkypeApp : MonoBehaviour, IWindowedApp, IPointerDownHandler
    {
        [Header("UI")]
        [SerializeField] public TextMeshProUGUI currentChatName;
        [SerializeField] public float messageScale;
    
        [Header("Scammer ChatBox")] 
        [SerializeField] public string scammerName;
        [SerializeField] public GameObject scammerChat; 
        [SerializeField] public GameObject scammerChatContent;
        [SerializeField] public Image scammerProfileToolPanel;
        [SerializeField] public GameObject scammerNotificationIcon;

        [Header("Hacker ChatBox")] 
        [SerializeField] public string hackerName;
        [SerializeField] public GameObject hackerChat;
        [SerializeField] public GameObject hackerChatContent;
        [SerializeField] public Image hackerProfileToolPanel;
        [SerializeField] public GameObject hackerNotificationIcon;
    
        [Header("Open/Close Window")]
        [SerializeField] private Button closeButton;
        [SerializeField] private float scaleOnOpen;
        [SerializeField] private float scaleOnClose;
        [SerializeField] private float openDuration;

        [Header("Skype Icon")]
        [SerializeField] private GameObject notificationIcon;
        
        private TextMeshProUGUI notificationCounterText;
        private int notificationCounter;
        private bool isOpen;
        private Vector3 initPos;
        
        private AudioManager audioManager;
        private FMODEvents FMODEvents;

        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        private void Start()
        {
            isOpen = false;
            gameObject.SetActive(false);
            notificationIcon.SetActive(false);

            notificationCounterText = notificationIcon.GetComponentInChildren<TextMeshProUGUI>();
            notificationCounter = 0;
            notificationCounterText.text = notificationCounter.ToString();
            initPos = gameObject.transform.localPosition;
            
            SkampMessageSender.MessageRecieved += OnNewNotificationGet;
            closeButton.onClick.AddListener(CloseApp);
        }
        
        private void OnDestroy()
        {
            SkampMessageSender.MessageRecieved -= OnNewNotificationGet;
            closeButton.onClick.RemoveAllListeners();
        }

        private async UniTask CloseAppAsync()
        {
            closeButton.interactable = false;
        
            await transform.DOScale(scaleOnClose, openDuration).ToUniTask();
            gameObject.transform.localScale = new Vector3(scaleOnClose, scaleOnClose, scaleOnClose);

            closeButton.interactable = true;
            gameObject.SetActive(false);
            isOpen = false;
            gameObject.transform.localPosition = initPos;
        }

        private async UniTask OpenAppAsync()
        {
            gameObject.SetActive(true);
            closeButton.interactable = false;
        
            await transform.DOScale(scaleOnOpen, openDuration).ToUniTask();
            gameObject.transform.localScale = new Vector3(scaleOnOpen, scaleOnOpen, scaleOnOpen);

            closeButton.interactable = true;
        
            notificationIcon.SetActive(false);
            notificationCounter = 0;
            isOpen = true;
        }

        private void OnNewNotificationGet()
        {
            audioManager.PlayOneShot(FMODEvents.skypeMessageSound);

            if (isOpen)
            {
                return;
            }
            
            notificationCounter++;
            notificationCounterText.text = notificationCounter.ToString();

            notificationIcon.SetActive(true);
            notificationIcon.transform.DOScale(1.3f, 0.1f).SetLoops(2, LoopType.Yoyo);
        }

        public void CloseApp()
        {
            if (isOpen)
            {
                CloseAppAsync().Forget();
            }
        }

        public void OpenApp()
        {
            if (!isOpen)
            {
                gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
                OpenAppAsync().Forget();
            }
            else
            {
                CloseApp();
            }
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
        }
    }
}
