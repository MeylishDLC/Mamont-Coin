using Cysharp.Threading.Tasks;
using Script.Data;
using Script.Managers;
using Script.Sound;

namespace Script.Core.Buffs
{
    public class MultiplyMultiplierBuffTimeLimited: TimeLimitedBuff
    {
        public override void BuyBuff()
        {
            BuyTimesMultiplierTimeLimitedAsync().Forget();
        }

        private async UniTask BuyTimesMultiplierTimeLimitedAsync()
        { 
            if (DataBank.Clicks >= price)
            {
                AudioManager.PlayOneShot(FMODEvents.buySound);
                DataBank.Multiplier *= buffAmount;
                var currentMultiplier = DataBank.Multiplier;
                
                DataBank.Clicks -= price;
            
                ClickHandler.ClicksUpdated?.Invoke(-price);
                
                await UniTask.Delay(timeLimitMilliseconds);

                var diff = DataBank.Multiplier - currentMultiplier;
                if (diff > 0)
                {
                    DataBank.Multiplier = currentMultiplier/buffAmount + diff;
                }
                else
                {
                    DataBank.Multiplier /= buffAmount;
                }
            }
        }
    }
}