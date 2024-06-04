using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Script.Core.Boosts
{
    [CreateAssetMenu(fileName = "AutoClicker", menuName = "Boosts/AutoClicker")]
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
                OnBoostAddClicks.Invoke(addAmount);
            }
        }
        
    }
}