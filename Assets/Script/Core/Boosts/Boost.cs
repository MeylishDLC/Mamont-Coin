using System;
using System.Numerics;
using UnityEngine;

namespace Script.Core.Boosts
{
    public class Boost: ScriptableObject
    {
        protected bool IsEnabled { get; set; }
        public static Action<BigInteger> OnBoostAddClicks;

        public virtual void Activate()
        {
            throw new NotImplementedException();
        }

        public virtual void Disable() { IsEnabled = false; }
    }
}