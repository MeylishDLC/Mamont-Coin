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
    public abstract class Popup: MonoBehaviour, IWindowedApp
    {
        [SerializeField] protected int AppearIntervalMilliseconds;
        [SerializeField] private bool destroyOnClose;
        [SerializeField] private Button closeButton;
        public bool isActive { get; set; }
        protected PopupsService PopupsService { get; private set; }
        protected AudioManager AudioManager { get; private set; }
        protected FMODEvents FMODEvents { get; private set; }
        
        [Inject]
        public void Construct(PopupsService popupsService, AudioManager audioManager, FMODEvents fmodEvents)
        {
            PopupsService = popupsService;
            AudioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        private void Start()
        {
            closeButton.onClick.AddListener(CloseApp);
        }
        public virtual void PopupAppear(){}
        public virtual void OpenApp()
        {
            gameObject.SetActive(true);
            transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo);
        }

        public virtual void CloseApp()
        {
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