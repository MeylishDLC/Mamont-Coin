using Script.Data;

namespace Script.Core.Buffs
{
    public class AddMultiplierBuff: Buff
    {
        public void BuyAddMultiplier()
        {
            if (DataBank.Clicks >= price)
            {
                DataBank.Multiplier += buffAmount;
                DataBank.Clicks -= price;
            
                Events.ClicksUpdated?.Invoke();
            }
        }
    }
}