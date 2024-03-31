using Cysharp.Threading.Tasks;

namespace Script.Core.Buffs
{
    public class MultiplyMultiplierBuffTimeLimited: TimeLimitedBuff
    {
        private async UniTask BuyTimesMultiplierTimeLimitedAsync()
        { 
            if (GameManager.Clicks >= price)
            {
                GameManager.Multiplier *= buffAmount;
                var currentMultiplier = GameManager.Multiplier;
                
                GameManager.Clicks -= price;
            
                Events.ClicksUpdated?.Invoke();
                await UniTask.Delay(timeLimitMilliseconds);

                var diff = GameManager.Multiplier - currentMultiplier;
                if (diff > 0)
                {
                    GameManager.Multiplier = currentMultiplier/buffAmount + diff;
                }
                else
                {
                    GameManager.Multiplier /= buffAmount;
                }
            }
        }

        public void BuyTimesMultiplierTimeLimited()
        {
            BuyTimesMultiplierTimeLimitedAsync().Forget();
        }
    }
}