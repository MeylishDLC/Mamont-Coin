using System.Linq;
using System.Numerics;
using Script.Core;
using Script.Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Script.Managers
{
    public class GameEventsHandler : MonoBehaviour
    {
        [SerializeField] private SerializedDictionary<int, UnityEvent> clickCountEventPair;
        [SerializeField] private GameManager gameManager;
        
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

                    if (goalIndex == clickCountEventPair.Count)
                    {
                        gameManager.GameEnd();
                        goalIndex++;
                    }
                    Debug.Log($"Current goal: {currentGoal}");
                }
            }
            else
            {
                Debug.Log("Game events all goals reached");
            }
        }

    }
}
