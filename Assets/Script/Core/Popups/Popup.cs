using System;
using System.Collections.Generic;
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
        [SerializeField] private bool destroyOnClose;
        [SerializeField] private Button closeButton;
        protected AudioManager AudioManager { get; private set; }
        protected FMODEvents FMODEvents { get; private set; }

        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            AudioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        public virtual void OpenApp()
        {
            closeButton.onClick.AddListener(CloseApp);
            
            gameObject.SetActive(true);
            transform.localScale = new Vector3(1, 1, 1);
            transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        public virtual void CloseApp()
        {
            closeButton.onClick.RemoveAllListeners();
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