namespace Script.Core.Buffs
{
    public class AddMultiplierBuff: Buff
    {
        public void BuyAddMultiplier()
        {
            if (GameManager.Clicks >= price)
            {
                GameManager.Multiplier += buffAmount;
                GameManager.Clicks -= price;
            
                Events.ClicksUpdated?.Invoke();
            }
        }
    }
}