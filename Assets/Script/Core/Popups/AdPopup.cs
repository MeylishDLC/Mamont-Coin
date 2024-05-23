using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Sound;
using Script.UI;
using UnityEngine;
using Zenject;

namespace Script.Core.Popups
{
    public class AdPopup: Popup
    {
        public override void PopupAppear()
        {
            PopupWindowAppearAsync().Forget();
        }
        private async UniTask PopupWindowAppearAsync()
        {
            isActive = true;
            while (isActive)
            {
                var popupWindow = PopupsService.SpawnRandomly(gameObject);
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
                await UniTask.Delay(AppearIntervalMilliseconds);
            }
        }
    }
}