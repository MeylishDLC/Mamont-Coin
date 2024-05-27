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
        
        private EventInstance callMusicInstance;
        private Button button;

        public static event Action OnDuralingoCallClicked; 
        public override void OpenApp()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
            
            gameObject.SetActive(true);
            callMusicInstance = AudioManager.CreateInstance(FMODEvents.skypeCallSound);
            callMusicInstance.start();
        }

        private void OnClick() => OnDuralingoCallClicked?.Invoke();
        public override void CloseApp()
        {
            callMusicInstance.stop(STOP_MODE.IMMEDIATE);
            Destroy(gameObject);
        }
        
    }
}