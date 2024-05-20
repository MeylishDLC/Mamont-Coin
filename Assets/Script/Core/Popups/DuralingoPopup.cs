using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Script.Managers;
using UnityEngine;

namespace Script.Core.Popups
{
    [CreateAssetMenu]
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
                Bounce(popupWindow, bounceCts.Token).Forget();
                
                await UniTask.Delay(AppearIntervalMilliseconds);
                bounceCts.Cancel();
                
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