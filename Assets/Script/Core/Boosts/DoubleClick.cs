using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Core.Boosts
{
    [CreateAssetMenu(fileName = "DoubleClick", menuName = "Boosts/DoubleClick")]
    public class DoubleClick: Boost, IImprovableBoost
    {
        [field:SerializeField] public float PercentageOfDoubleClick { get; private set; }
        [field: SerializeField] public int DoubleClickAmount { get; private set; } = 2;
        
        [Header("Boost Improve")]
        [field:SerializeField] public string ImproveText { get; set; }
        [field: SerializeField] public int DoubleClickImproveAmount { get; private set; }

        private int currentDoubleClickAmount;
        public override void Activate()
        {
            currentDoubleClickAmount = DoubleClickAmount;
            IsEnabled = true;
            ClickHandler.ClicksUpdated += OnClick;
        }
        public void Improve()
        {
            currentDoubleClickAmount = DoubleClickImproveAmount;
        }

        public override void Disable()
        {
            base.Disable();
            ClickHandler.ClicksUpdated -= OnClick;
        }

        private bool DoubleClickChance()
        {
            var chance = Random.Range(1, 100);
            if (chance <= PercentageOfDoubleClick)
            {
                return true;
            }
            return false;
        }
        private void OnClick(BigInteger addAmount)
        {
            if (!DoubleClickChance())
            {
                return;
            }

            addAmount *= currentDoubleClickAmount;
            Debug.Log($"Double click = {addAmount}, {currentDoubleClickAmount}");
            OnBoostAddClicks.Invoke(addAmount);
        }
    }
}