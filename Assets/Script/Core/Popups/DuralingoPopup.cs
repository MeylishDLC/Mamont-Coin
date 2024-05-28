using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using FMOD.Studio;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Core.Popups
{
    public class DuralingoPopup: Popup
    {
        [field: SerializeField] public int DuralingoCallsAmount { get; private set; }
        
        private Button button;
        public static event Action OnDuralingoCallClicked; 
        public override void OpenApp()
        {
            isOpen = true;
            GameManager.OnGameEnd += CloseApp;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
            
            gameObject.SetActive(true);
            
            AudioManager.InitializeMusic("Skype Call", FMODEvents.skypeCallSound);
        }

        private void OnClick() => OnDuralingoCallClicked?.Invoke();
        public override void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            
            AudioManager.StopMusic("Skype Call", STOP_MODE.IMMEDIATE);
            GameManager.OnGameEnd -= CloseApp;
            Destroy(gameObject);
        }
    }
}