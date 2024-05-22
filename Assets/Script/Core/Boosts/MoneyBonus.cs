using System;
using Script.Data;
using UnityEngine;

namespace Script.Core.Boosts
{
    [CreateAssetMenu(fileName = "MoneyBonus", menuName = "Boosts/MoneyBonus")]
    public class MoneyBonus: Boost
    {
        [field:SerializeField] public int Amount { get; private set; }
        public override void Activate()
        {
            var addAmount = Amount;
            DataBank.Clicks += addAmount;
            ClickHandler.ClicksUpdated?.Invoke(addAmount);
        }
    }
}