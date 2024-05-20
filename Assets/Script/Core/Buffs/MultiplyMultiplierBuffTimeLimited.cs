using Cysharp.Threading.Tasks;
using Script.Data;

namespace Script.Core.Buffs
{
    public class MultiplyMultiplierBuffTimeLimited: TimeLimitedBuff
    {
        private async UniTask BuyTimesMultiplierTimeLimitedAsync()
        { 
            if (DataBank.Clicks >= price)
            {
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

        public void BuyTimesMultiplierTimeLimited()
        {
            BuyTimesMultiplierTimeLimitedAsync().Forget();
        }
    }
}