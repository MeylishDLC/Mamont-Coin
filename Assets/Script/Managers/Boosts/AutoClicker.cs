using Cysharp.Threading.Tasks;

namespace Script.Core
{
    public class AutoClicker : Boost
    {
        public int ClickFrequencyMilliseconds { get; set; }
        public int AutoClickAmount { get; set; }
        
        public AutoClicker(int clickFrequencyMilliseconds, int autoClickAmount)
        {
            ClickFrequencyMilliseconds = clickFrequencyMilliseconds;
            AutoClickAmount = 1;
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
                GameManager.Clicks += AutoClickAmount;
                Events.ClicksUpdated?.Invoke();
            }
        }
    }
}