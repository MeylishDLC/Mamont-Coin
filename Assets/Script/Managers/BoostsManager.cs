using System.Collections.Generic;
using Script.Core;
using Script.Core.Boosts;
using Script.Data;
using UnityEngine;

namespace Script.Managers
{
    public class BoostsManager
    {
        public string SpecificBoostName;
        
        private List<Boost> boosts;
        private IImprovableBoost specificBoost;

        public BoostsManager(List<Boost> boosts)
        {
            this.boosts = boosts;
        }
        
        public void EnableBoost(Boost boost)
        {
            boost.Activate();
            if (boost is IImprovableBoost improvableBoost)
            {
                SpecificBoostName = improvableBoost.ImproveText;
                specificBoost = improvableBoost;
            }
        }
        
        public void DisableAllBoosts()
        {
            foreach (var boost in boosts)
            {
                boost.IsEnabled = false;
            }
        }
        public void SpecificBoost()
        {
            specificBoost.Improve();
        }
    }
}
