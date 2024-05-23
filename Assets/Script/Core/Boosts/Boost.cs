using Script.Data;
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
        }
        public virtual void Activate(){}
    }
}