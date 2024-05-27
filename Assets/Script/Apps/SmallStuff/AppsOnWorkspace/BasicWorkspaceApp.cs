using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace
{
    public class BasicWorkspaceApp: MonoBehaviour, IWindowedApp
    {
        [SerializeField] protected Button openIconButton;
        [SerializeField] protected Button closeButton; 
        [SerializeField] protected float scaleOnClose;
        [SerializeField] protected float scaleDuration;
        protected bool isOpen;
        protected virtual void Start()
        {
            closeButton.onClick.AddListener(CloseApp);
            openIconButton.onClick.AddListener(OpenApp);
            gameObject.SetActive(false);
        }

        public virtual void OpenApp()
        {
            if (!isOpen)
            {
                OpenAppAsync().Forget();
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
            
            openIconButton.interactable = true;
            closeButton.interactable = true;
        }
    }
}