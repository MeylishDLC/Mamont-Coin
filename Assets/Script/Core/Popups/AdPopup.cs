using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.UI;
using UnityEngine;

namespace Script.Core.Popups
{
    [CreateAssetMenu]
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
                var popupWindow = PopupsManager.Instance.RandomSpawn(Popups);
                popupWindow.GetComponent<PopupWindow>().isPaid = false;
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
                await UniTask.Delay(AppearIntervalMilliseconds);
            }
        }
    }
}