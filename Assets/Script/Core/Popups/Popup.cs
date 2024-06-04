using DG.Tweening;
using Script.Managers;
using Script.Sound;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Core.Popups
{
    public class Popup: MonoBehaviour, IWindowedApp
    {
        [SerializeField] protected bool destroyOnClose;
        [SerializeField] protected Button closeButton;
        [SerializeField] protected bool disabledOnStart;
        protected AudioManager AudioManager { get; private set; }
        protected FMODEvents FMODEvents { get; private set; }
        protected bool isOpen;

        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            AudioManager = audioManager;
            FMODEvents = fmodEvents;
        }

        private void Start()
        {
            if (disabledOnStart)
            {
                gameObject.SetActive(false);
            }
        }

        public virtual void OpenApp()
        {
            if (isOpen)
            {
                return;
            }
            
            isOpen = true;
            
            closeButton.onClick.AddListener(CloseApp);
            
            gameObject.SetActive(true);
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        public virtual void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            
            closeButton.onClick.RemoveAllListeners();
            isOpen = false;
            if (destroyOnClose)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        
    }
    
}