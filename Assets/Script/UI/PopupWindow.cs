using System;
using DG.Tweening;
using Script.Core;
using Script.Data;
using Script.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class PopupWindow : MonoBehaviour
    {
        public static event Action OnPaidPopupClose;
        
        [Header("Settings")]
        [SerializeField] private bool destroyOnClose;
        [SerializeField] private Button closeButton;
        public bool isPaid;

        private void Start()
        {
            closeButton.onClick.AddListener(CloseWindow);
        }

        public void ShowWindow()
        {
            gameObject.SetActive(true);
            transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        public void CloseWindow()
        {
            if (destroyOnClose)
            {
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
            
            if (isPaid)
            {
                OnPaidPopupClose?.Invoke();
            }
        }
    }
}
