using System;
using System.Collections.Generic;
using System.Linq;
using Script.Core;
using Script.Core.Boosts;
using Script.Data;
using UnityEngine;

namespace Script.Managers
{
    public class BoostsService
    {
        public string SpecificBoostName;
        public List<Boost> BoostsToManage;
        
        private IImprovableBoost specificBoost;
        
        public void EnableBoost(Boost boost)
        {
            boost.Activate();
            
            if (boost is not IImprovableBoost improvableBoost)
            {
                return;
            }
            
            SpecificBoostName = improvableBoost.ImproveText;
            specificBoost = improvableBoost;
        }
        
        public void DisableAllBoosts()
        {
            if (!BoostsToManage.Any())
            {
                Debug.LogError("No manageable boosts set. Cannot disable");
                return;
            }
            
            foreach (var boost in BoostsToManage)
            {
                boost.IsEnabled = false;
            }
        }
        public void SpecificBoost()
        {
            if (specificBoost is null)
            {
                throw new Exception("Specific boost was null");
            }
            
            specificBoost.Improve();
        }
    }
}
