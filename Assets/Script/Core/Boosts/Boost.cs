using System;
using System.Numerics;
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
        public static Action<BigInteger> OnBoostAddClicks;
        private void OnDestroy()
        {
        }

        public virtual void Activate()
        {
        }

        public virtual void Disable()
        {
            IsEnabled = false;
        }
    }
}