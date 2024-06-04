namespace Script.Core.Buffs
{
    public class MultiplyMultiplierBuff: Buff
    {
        public override void BuyBuff()
        {
            if (DataBank.Clicks >= price)
            {
                AudioManager.PlayOneShot(FMODEvents.buySound);
                DataBank.Multiplier *= buffAmount;
                DataBank.Clicks -= price;
            
                ClickHandler.ClicksUpdated?.Invoke(-price);
            }
        }
    }
}