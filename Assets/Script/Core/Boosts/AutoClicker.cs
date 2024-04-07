using Cysharp.Threading.Tasks;
using Script.Data;

namespace Script.Core
{
    public class AutoClicker : Boost
    {
        public int ClickFrequencyMilliseconds { get; set; }
        public int AutoClickAmount { get; set; }
        
        public AutoClicker(int clickFrequencyMilliseconds, int autoClickAmount = 1)
        {
            ClickFrequencyMilliseconds = clickFrequencyMilliseconds;
            AutoClickAmount = autoClickAmount;
        }
        
        public override void Activate()
        {
            IsEnabled = true;
            AutoClickAsync().Forget();
        }
        
        private async UniTask AutoClickAsync()
        {
            while (IsEnabled)
            {
                await UniTask.Delay(ClickFrequencyMilliseconds);
                DataBank.Clicks += AutoClickAmount;
                Events.ClicksUpdated?.Invoke();
            }
        }
    }
}