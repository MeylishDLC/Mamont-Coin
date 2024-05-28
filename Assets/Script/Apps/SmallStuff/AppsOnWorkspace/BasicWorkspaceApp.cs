using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace
{
    public class BasicWorkspaceApp: MonoBehaviour, IWindowedApp, IPointerDownHandler
    {
        [SerializeField] protected Button openIconButton;
        [SerializeField] protected Button closeButton; 
        [SerializeField] protected float scaleOnClose;
        [SerializeField] protected float scaleDuration;

        protected Vector3 initPos;
        protected bool isOpen;
        protected virtual void Start()
        {
            GameManager.OnGameEnd += CloseApp;
            closeButton.onClick.AddListener(CloseApp);
            openIconButton.onClick.AddListener(OpenApp);
            
            gameObject.transform.localScale = new Vector3(scaleOnClose, scaleOnClose, 0);
            initPos = gameObject.transform.localPosition;
            
            gameObject.SetActive(false);
        }
        private void OnDestroy()
        {
            GameManager.OnGameEnd -= CloseApp;
        }
        public virtual void OpenApp()
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

        private async UniTask OpenAppAsync()
        {
            openIconButton.interactable = false;
            closeButton.interactable = false;
            
            gameObject.SetActive(true);
            await gameObject.transform.DOScale(1, scaleDuration).ToUniTask();
            isOpen = true;
            
            openIconButton.interactable = true;
            closeButton.interactable = true;
        }
        public virtual void CloseApp()
        {
            if (isOpen)
            {
                CloseAppAsync().Forget();
            }
        }
        private async UniTask CloseAppAsync()
        {
            openIconButton.interactable = false;
            closeButton.interactable = false;
            
            await gameObject.transform.DOScale(scaleOnClose, scaleDuration).ToUniTask();
            gameObject.SetActive(false);
            isOpen = false;
            gameObject.transform.localPosition = initPos;
            openIconButton.interactable = true;
            closeButton.interactable = true;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
        }
    }
}