using UnityEngine;

namespace Script.Core
{
    public abstract class Boost
    {
        public bool IsEnabled { get; set; }
        public abstract void Activate();
    }
}