using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Script.Core
{
    public class PaidPopups: Boost
    {
        public int PaidAppearFrequencyMilliseconds { get; set; }

        private List<GameObject> adWindows;

        public PaidPopups(int paidAppearFrequencyMilliseconds, List<GameObject> popupAdWindows)
        {
            PaidAppearFrequencyMilliseconds = paidAppearFrequencyMilliseconds;
            adWindows = popupAdWindows;
        }
        public override void Activate()
        {
            IsEnabled = true;
            PaidPopupWindowAppearAsync().Forget();
        }
        
        private async UniTask PaidPopupWindowAppearAsync()
        {
            while (IsEnabled)
            {
                var popupWindow = PopupsManager.GetInstance().RandomSpawn(adWindows);
            
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
                await UniTask.Delay(PaidAppearFrequencyMilliseconds);
            }
        }
    }
}