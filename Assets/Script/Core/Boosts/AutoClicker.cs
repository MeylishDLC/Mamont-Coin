using Cysharp.Threading.Tasks;
using Script.Data;
using UnityEngine;

namespace Script.Core.Boosts
{
    [CreateAssetMenu]
    public class AutoClicker : Boost, IImprovableBoost
    {
        [field: SerializeField] public int ClickFrequencyMilliseconds { get; private set; }
        [field: SerializeField] public int AutoClickAmount { get; private set; } = 1;
        
        [Header("Boost Improve")]
        [field:SerializeField] public string ImproveText { get; set; }

        [field: SerializeField] public int AutoClickImproveAmount { get; private set; }
        
        public override void Activate()
        {
            IsEnabled = true;
            AutoClickAsync().Forget();
        }
        public void Improve()
        {
            AutoClickAmount = AutoClickImproveAmount;
        }
        private async UniTask AutoClickAsync()
        {
            while (IsEnabled)
            {
                await UniTask.Delay(ClickFrequencyMilliseconds);
                var addAmount = AutoClickAmount;
                DataBank.Clicks += addAmount;
                ClickHandler.ClicksUpdated?.Invoke(addAmount);
            }
        }
        
    }
}