using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Script.Core;
using Script.Core.Popups;
using Script.Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Script.Managers
{
    public class GameEventsHandler : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<int, UnityEvent> clickCountEventPair;
        
        private int goalIndex;
        private int currentGoal;

        private IDataBank dataBank;
        
        [Inject]
        public void Construct(IDataBank dataBank)
        {
            this.dataBank = dataBank;
        }
        private void Start()
        {
            goalIndex = 0;
            currentGoal = clickCountEventPair.Keys.ElementAt(goalIndex);
            ClickHandler.ClicksUpdated += CheckEnoughClicks;
        }
        private void OnDestroy()
        {
            ClickHandler.ClicksUpdated -= CheckEnoughClicks;
        }

        private void CheckEnoughClicks(BigInteger addAmount)
        {
            if (goalIndex < clickCountEventPair.Count)
            {
                if (dataBank.Clicks >= currentGoal)
                {
                    var keys = clickCountEventPair.Keys.ToArray();
                    clickCountEventPair[keys[goalIndex]]?.Invoke();
                    goalIndex++;
                    if (goalIndex < keys.Length)
                    {
                        currentGoal = keys[goalIndex];
                    }
                    //Debug.Log($"current goal: {currentGoal}");
                }
            }
            else
            {
                Debug.Log("Game events all goals reached");
            }
        }

    }
}
