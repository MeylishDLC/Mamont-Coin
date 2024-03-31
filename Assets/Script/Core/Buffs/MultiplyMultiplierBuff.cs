namespace Script.Core.Buffs
{
    public class MultiplyMultiplierBuff: Buff
    {
        public void BuyMultiplyMultiplierBuff()
        {
            if (GameManager.Clicks >= price)
            {
                GameManager.Multiplier *= buffAmount;
                GameManager.Clicks -= price;
            
                Events.ClicksUpdated?.Invoke();
            }
        }
    }
}