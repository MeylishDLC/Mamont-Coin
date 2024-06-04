using System;
using Script.Core.Boosts;

namespace Script.Managers
{
    public class SpecificBoostSetter
    {
        public string SpecificBoostName;
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
