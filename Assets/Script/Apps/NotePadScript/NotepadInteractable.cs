using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Script.Apps.NotePadScript
{
    public class NotepadInteractable : MonoBehaviour, IWindowedApp, IPointerDownHandler
    {
        [Header("Open/Close Window")]
        [SerializeField] private Button closeButton;
        [SerializeField] private float scaleOnOpen;
        [SerializeField] private float scaleOnClose;
        [SerializeField] private float openDuration;
    
        [Header("Chosen Boosts")] 
        [SerializeField] private List<TMP_Text> chosenBoosts;
        
        private int chosenBoostAmount;
        private bool isOpen;
        private Vector3 initPos;
        private void Start()
        {
            gameObject.SetActive(false);

            foreach (var boostInfo in chosenBoosts)
            {
                boostInfo.gameObject.SetActive(false);
            }
        
            closeButton.onClick.AddListener(CloseApp);
            initPos = gameObject.transform.localPosition;
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveAllListeners();
        }

        public void WriteDownNewBoost(string boostName)
        {
            chosenBoostAmount++;
            chosenBoosts[chosenBoostAmount - 1].text = boostName;
            chosenBoosts[chosenBoostAmount - 1].gameObject.SetActive(true);
        }
    
    
        public void OpenApp()
        {
            if (isOpen)
            {
                CloseApp();
            }
            else
            {
                gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
                OpenAppAsync().Forget();
            }
        }
    
        public void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            CloseAppAsync().Forget();
        }
    
        private async UniTask OpenAppAsync()
        {
            gameObject.SetActive(true);
            closeButton.interactable = false;
        
            await transform.DOScale(scaleOnOpen, openDuration).ToUniTask();

            closeButton.interactable = true;
            isOpen = true;
        }
    
        private async UniTask CloseAppAsync()
        {
            closeButton.interactable = false;
        
            await transform.DOScale(scaleOnClose, openDuration).ToUniTask();

            closeButton.interactable = true;
            gameObject.SetActive(false);
            gameObject.transform.localPosition = initPos;
            isOpen = false;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
        }
    }
}
