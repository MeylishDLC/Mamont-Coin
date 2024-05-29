using System;
using System.Numerics;
using Script.Data;
using Script.Managers;
using TMPro;
using UnityEngine;

namespace Script.Core
{
    public class ClickHandler
    { 
        public static Action<BigInteger> ClicksUpdated;
        private IDataBank dataBank;

        public ClickHandler(IDataBank dataBank)
        {
            this.dataBank = dataBank;
        }
        public void Increment(BigInteger addAmount)
        {
            dataBank.Clicks += addAmount;
            ClicksUpdated?.Invoke(addAmount);
        }
    }
}
