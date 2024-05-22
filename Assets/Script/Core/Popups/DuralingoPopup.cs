using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using FMOD.Studio;
using Script.Managers;
using Script.Sound;
using UnityEngine;

namespace Script.Core.Popups
{
    [CreateAssetMenu(fileName = "DuralingoPopup", menuName = "Popups/DuralingoPopup")]
    public class DuralingoPopup: Popup
    {
        [field: SerializeField] public int DuralingoCallsAmount { get; private set; }
        [field: SerializeField] public Vector2 SpawnPosition { get; private set; }

        private CancellationTokenSource bounceCts;
        public override void PopupAppear()
        {
            PopupAppearAsync().Forget();
        }

        private async UniTask PopupAppearAsync()
        {
            isActive = true;
            for (int i = 0; i < DuralingoCallsAmount; i++)
            {
                var popupWindow = PopupsManager.Instance.SpawnRandomObject(SpawnPosition, Popups);
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();

                bounceCts = new CancellationTokenSource();
                
                var instance = AudioManager.instance.CreateInstance(FMODEvents.instance.skypeCallSound);
                instance.start();
                
                Bounce(popupWindow, bounceCts.Token).Forget();
                
                await UniTask.Delay(AppearIntervalMilliseconds);
       
                bounceCts.Cancel();
                instance.stop(STOP_MODE.IMMEDIATE);
                
                await popupWindow.transform.DOScale(0.9f, 0.2f).ToUniTask();
                Destroy(popupWindow);
                await UniTask.Delay(300);
            }
            isActive = false;
        }

        private async UniTask Bounce(GameObject popupWindow, CancellationToken token)
        {

            
            while (!token.IsCancellationRequested)
            {
                await popupWindow.transform.DOScale(0.95f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask(cancellationToken: token);
                await UniTask.Delay(100, cancellationToken: token);
            }

            bounceCts.Dispose();
        }
    }
}