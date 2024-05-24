using System;
using Script.Data;
using Script.Managers;
using UnityEngine;
using Zenject;

namespace Script.Core.Boosts
{
    public class Boost: ScriptableObject
    {
        public bool IsEnabled { get; set; }
        protected IDataBank DataBank { get; private set; }

        [Inject]
        public void Construct(IDataBank dataBank)
        {
            DataBank = dataBank;
            GameManager.OnGameEnd += Disable;
        }
        private void OnDestroy()
        {
            GameManager.OnGameEnd -= Disable;
        }
        public virtual void Activate(){}

        private void Disable()
        {
            IsEnabled = false;
        }
    }
}