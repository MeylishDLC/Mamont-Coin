using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Script.Core.Boosts
{
    [CreateAssetMenu]
    public class DoubleClick: Boost, IImprovableBoost
    {
        [field:SerializeField] public float PercentageOfDoubleClick { get; private set; }
        [field: SerializeField] public int DoubleClickAmount { get; private set; } = 2;
        
        [Header("Boost Improve")]
        [field:SerializeField] public string ImproveText { get; set; }

        [field: SerializeField] public int DoubleClickImproveAmount { get; private set; }
        
        public override void Activate()
        {
            IsEnabled = true;
            ClickHandler.ClicksUpdated += OnClick;
        }

        private void OnDisable()
        {
            ClickHandler.ClicksUpdated -= OnClick;
        }

        public void Improve()
        {
            DoubleClickAmount = DoubleClickImproveAmount;
        }

        private bool DoubleClickChance()
        {
            var chance = Random.Range(1, 100);
            if (chance <= PercentageOfDoubleClick)
            {
                Debug.Log($"Double click = {DoubleClickAmount}");
                return true;
            }
            return false;
        }

        private void OnClick(int addAmount)
        {
            if (!DoubleClickChance())
            {
                return;
            }
            
            addAmount *= addAmount - 1;
            ClickHandler.ClicksUpdated?.Invoke(addAmount);
        }
    }
}