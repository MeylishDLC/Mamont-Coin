using Script.Data;
using Script.Managers;
using Script.Sound;

namespace Script.Core.Buffs
{
    public class AddMultiplierBuff: Buff
    {
        public override void BuyBuff()
        {
            if (DataBank.Clicks >= price)
            {
                AudioManager.PlayOneShot(FMODEvents.buySound);
                
                DataBank.Multiplier += buffAmount;
                DataBank.Clicks -= price;
            
                ClickHandler.ClicksUpdated?.Invoke(-price);
            }
        }
    }
}