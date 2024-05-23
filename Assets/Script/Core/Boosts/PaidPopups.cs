using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core.Popups;
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
        [field:SerializeField] private List<PaidAdPopup> adWindows;
        public override void Activate()
        {
            PaidAdPopup.OnPaidPopupClick += PayForAd;
            IsEnabled = true;
            
            //отсылает команду сервису для спавна окон
        }

        private void PayForAd()
        {
            var addAmount = CoinsPerPopupWindow;
            DataBank.Clicks += addAmount;
            ClickHandler.ClicksUpdated?.Invoke(addAmount);
            Debug.Log("+ money for AD");
        }
    }
}