using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Data;
using Script.Managers;
using Script.Sound;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.ChatScript
{
    public class SkypeApp : MonoBehaviour, IWindowedApp
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
        private bool isOpen { get; set; }
    
        private void Start()
        {
            isOpen = false;
            gameObject.SetActive(false);
            notificationIcon.SetActive(false);

            notificationCounterText = notificationIcon.GetComponentInChildren<TextMeshProUGUI>();
            notificationCounter = 0;
            notificationCounterText.text = notificationCounter.ToString();

            Events.MessageRecieved += OnNewNotificationGet;
            closeButton.onClick.AddListener(CloseApp);
        }

        private async UniTask CloseAppAsync()
        {
            closeButton.interactable = false;
        
            await transform.DOScale(scaleOnClose, openDuration).ToUniTask();
            gameObject.transform.localScale = new Vector3(scaleOnClose, scaleOnClose, scaleOnClose);

            closeButton.interactable = true;
            gameObject.SetActive(false);
            isOpen = false;
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
            AudioManager.instance.PlayOneShot(FMODEvents.instance.skypeMessageSound);

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
            if (isOpen)
            {
                CloseAppAsync().Forget();   
            }
        }

        public void OpenApp()
        {
            if (!isOpen)
            {
                OpenAppAsync().Forget();
            }
        }
    
    
    }
}
