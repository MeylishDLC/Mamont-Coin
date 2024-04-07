using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script.Data;
using UnityEngine;
using UnityEngine.Events;

public class GameEventsHandler : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<int, UnityEvent> clickCountEventPair;
    
    private int goalIndex;
    private int currentGoal;

    #region Set Instance
    
    private static GameEventsHandler instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one GameEventsHandler in the scene.");
        }
        instance = this;
    }

    public static GameEventsHandler GetInstance()
    {
        return instance;
    }
    
    #endregion
    private void Start()
    {
        goalIndex = 0;
        currentGoal = clickCountEventPair.Keys.ElementAt(goalIndex);

        Events.ClicksUpdated += CheckEnoughClicks;
    }
    
    private void CheckEnoughClicks()
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
