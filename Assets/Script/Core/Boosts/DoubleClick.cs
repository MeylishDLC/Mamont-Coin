using UnityEngine;

namespace Script.Core
{
    public class DoubleClick: Boost
    {
        public float PercentageOfDoubleClick { get; set; }
        public int DoubleClickAmount { get; set; }
        
        public DoubleClick(float percentageChanceOfDoubleClick, int doubleClickAmount)
        {
            PercentageOfDoubleClick = percentageChanceOfDoubleClick;
            DoubleClickAmount = doubleClickAmount;
        }
        
        public override void Activate()
        {
            IsEnabled = true;
        }

        public int DoubleClickChance()
        {
            var chance = Random.Range(1, 100);
            if (chance <= PercentageOfDoubleClick)
            {
                Debug.Log($"Double click = {DoubleClickAmount}");
                return DoubleClickAmount;
            }
            return 1;
        }

    }
}