using UnityEngine;

namespace Script.Core.Boosts
{
    public class Boost: ScriptableObject
    {
        public bool IsEnabled { get; set; }
        public virtual void Activate(){}
    }
}