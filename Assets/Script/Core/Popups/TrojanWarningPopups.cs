using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Script.Core.Popups
{
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
                var popupWindow = PopupsService.SpawnRandomly(Popups);
                AudioManager.PlayOneShot(FMODEvents.errorSound);
            
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
                await UniTask.Delay(AppearIntervalMilliseconds);
            }
        } 
    }
}