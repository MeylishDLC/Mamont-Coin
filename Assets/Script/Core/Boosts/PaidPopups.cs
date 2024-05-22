using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Data;
using Script.Managers;
using Script.UI;
using UnityEngine;

namespace Script.Core.Boosts
{
    [CreateAssetMenu(fileName = "PaidPopups", menuName = "Boosts/PaidPopups")]
    public class PaidPopups: Boost
    {
        [field:SerializeField] public int PaidAppearFrequencyMilliseconds { get; private set; }
        [field:SerializeField] public int CoinsPerPopupWindow { get; private set; }
        [field:SerializeField] private List<GameObject> adWindows;
        public override void Activate()
        {
            PopupWindow.OnPaidPopupClose += PayForAd;
            IsEnabled = true;
            PaidPopupWindowAppearAsync().Forget();
        }

        private void PayForAd()
        {
            var addAmount = CoinsPerPopupWindow;
            DataBank.Clicks += addAmount;
            ClickHandler.ClicksUpdated?.Invoke(addAmount);
            Debug.Log("+ money for AD");
        }
        private async UniTask PaidPopupWindowAppearAsync()
        {
            while (IsEnabled)
            {
                var popupWindow = new GameObject();//PopupsManager.instance.RandomSpawn(adWindows);
            
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
                await UniTask.Delay(PaidAppearFrequencyMilliseconds);
            }
        }
    }
}