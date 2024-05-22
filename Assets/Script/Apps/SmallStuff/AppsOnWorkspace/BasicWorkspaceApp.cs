using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace
{
    public class BasicWorkspaceApp: MonoBehaviour, IWindowedApp
    {
        [SerializeField] private Button openIconButton;
        [SerializeField] private Button closeButton; 
        [SerializeField] private float scaleOnClose;
        [SerializeField] private float scaleDuration;
        private bool isOpen;
        private void Start()
        {
            closeButton.onClick.AddListener(CloseApp);
            openIconButton.onClick.AddListener(OpenApp);
            gameObject.SetActive(false);
        }

        public void OpenApp()
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
        public void CloseApp()
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