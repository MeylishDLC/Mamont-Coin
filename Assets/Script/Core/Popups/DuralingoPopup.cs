using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using FMOD.Studio;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Script.Core.Popups
{
    public class DuralingoPopup: Popup
    {
        [field: SerializeField] public int DuralingoCallsAmount { get; private set; }
        private EventInstance callMusicInstance;
        public override void OpenApp()
        {
            gameObject.SetActive(true);
            callMusicInstance = AudioManager.CreateInstance(FMODEvents.skypeCallSound);
            callMusicInstance.start();
        }
        public override void CloseApp()
        {
            callMusicInstance.stop(STOP_MODE.IMMEDIATE);
            Destroy(gameObject);
        }
    }
}