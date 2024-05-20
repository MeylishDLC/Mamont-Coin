using Script.Data;

namespace Script.Core.Buffs
{
    public class MultiplyMultiplierBuff: Buff
    {
        public void BuyMultiplyMultiplierBuff()
        {
            if (DataBank.Clicks >= price)
            {
                DataBank.Multiplier *= buffAmount;
                DataBank.Clicks -= price;
            
                ClickHandler.ClicksUpdated?.Invoke(-price);
            }
        }
    }
}