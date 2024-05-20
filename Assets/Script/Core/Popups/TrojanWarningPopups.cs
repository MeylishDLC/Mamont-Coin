using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Sound;
using UnityEngine;

namespace Script.Core.Popups
{
    [CreateAssetMenu]
    public class TrojanWarningPopups: Popup
    {
        public override void PopupAppear()
        {
            PopupAppearAsync().Forget();
        }
        private async UniTask PopupAppearAsync()
        {
            isActive = true;
            while (isActive)
            {
                var popupWindow = PopupsManager.Instance.RandomSpawn(Popups);
                AudioManager.instance.PlayOneShot(FMODEvents.instance.errorSound);
            
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
                await UniTask.Delay(AppearIntervalMilliseconds);
            }
        } 
    }
}