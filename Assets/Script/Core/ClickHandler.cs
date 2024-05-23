using System;
using Script.Data;
using Script.Managers;
using TMPro;
using UnityEngine;

namespace Script.Core
{
    public class ClickHandler
    { 
        public static Action<int> ClicksUpdated;
        private IDataBank dataBank;

        public ClickHandler(IDataBank dataBank)
        {
            this.dataBank = dataBank;
        }
        public void Increment(long addAmount)
        {
            dataBank.Clicks += addAmount;
            ClicksUpdated?.Invoke((int)addAmount);
        }
    }
}
