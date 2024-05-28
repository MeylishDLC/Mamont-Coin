using System;
using FMODUnity;
using Script.Data;
using Script.Managers;
using UnityEngine;
using Zenject;

namespace Script.Core.Boosts
{
    public class Boost: ScriptableObject
    {
        protected bool IsEnabled { get; set; }
        public static Action<int> OnBoostAddClicks;
        private void OnDestroy()
        {
            GameManager.OnGameEnd -= Disable;
        }

        public virtual void Activate()
        {
            GameManager.OnGameEnd += Disable;
        }

        private void Disable()
        {
            IsEnabled = false;
        }
    }
}