using Script.Core.Popups;
using Script.Core.Popups.Spawns;
using UnityEngine;

namespace Script.Core.Boosts
{
    [CreateAssetMenu(fileName = "PaidPopups", menuName = "Boosts/PaidPopups")]
    public class PaidPopups: Boost
    {
        [field:SerializeField] public int CoinsPerPopupWindow { get; private set; }
        [SerializeField] private RandomSpawner paidAdSpawner;
        public override void Activate()
        {
            PaidAdPopup.OnPaidPopupClick += PayForAd;
            IsEnabled = true;
            paidAdSpawner.StartSpawn();
        }

        public override void Disable()
        {
            base.Disable();
            paidAdSpawner.StopSpawn();
            PaidAdPopup.OnPaidPopupClick -= PayForAd;
        }
        private void PayForAd()
        {
            var addAmount = CoinsPerPopupWindow;
            OnBoostAddClicks.Invoke(addAmount);
            Debug.Log("+ money for AD");
        }
    }
}