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
                AudioManager.instance.PlayOneShot(FMODEvents.instance.buySound);
                DataBank.Multiplier += buffAmount;
                DataBank.Clicks -= price;
            
                ClickHandler.ClicksUpdated?.Invoke(-price);
            }
        }
    }
}