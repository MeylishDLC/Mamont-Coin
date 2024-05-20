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
        private readonly ProgressHandler progressHandler;
        public ClickHandler(ProgressHandler progressHandler)
        {
            this.progressHandler = progressHandler;
        }
        
        public void Increment(long addAmount)
        {
            DataBank.Clicks += addAmount;
            progressHandler.AddProgress(addAmount);
            ClicksUpdated?.Invoke((int)addAmount);
        }
    }
}
