using System;
using System.Collections.Generic;
using System.Linq;
using Script.Core;
using Script.Core.Popups;
using Script.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Managers
{
    public class GameEventsHandler : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<int, UnityEvent> clickCountEventPair;
        
        private int goalIndex;
        private int currentGoal;

        #region Set Instance
    
        public static GameEventsHandler instance { get; private set; }
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one GameEventsHandler in the scene.");
            }
            instance = this;
        }
        
        #endregion
        private void Start()
        {
            goalIndex = 0;
            currentGoal = clickCountEventPair.Keys.ElementAt(goalIndex);
        }

        private void OnEnable()
        {
            ClickHandler.ClicksUpdated += CheckEnoughClicks;
        }

        private void OnDestroy()
        {
            ClickHandler.ClicksUpdated -= CheckEnoughClicks;
        }

        private void CheckEnoughClicks(int addAmount)
        {
            if (goalIndex < clickCountEventPair.Count)
            {
                if (DataBank.Clicks >= currentGoal)
                {
                    var keys = clickCountEventPair.Keys.ToArray();
                    clickCountEventPair[keys[goalIndex]]?.Invoke();
                    goalIndex++;
                    if (goalIndex < keys.Length)
                    {
                        currentGoal = keys[goalIndex];
                    }
                }
            }
            else
            {
                Debug.Log("All goals achieved");
            }
        }

    }
}
