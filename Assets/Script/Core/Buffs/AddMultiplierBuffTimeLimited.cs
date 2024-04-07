using Cysharp.Threading.Tasks;
using Script.Data;
using UnityEngine;

namespace Script.Core.Buffs
{
    public class AddMultiplierBuffTimeLimited: AddMultiplierBuff
    {
        [SerializeField] private int timeLimitMilliseconds;
        
        public void BuyAddMultiplierLimited()
        {
            BuyAddMultiplierLimitedAsync().Forget();
        }
        private async UniTask BuyAddMultiplierLimitedAsync()
        {
            BuyAddMultiplier();
            await UniTask.Delay(timeLimitMilliseconds);
            DataBank.Multiplier -= buffAmount;
        }
    }
}