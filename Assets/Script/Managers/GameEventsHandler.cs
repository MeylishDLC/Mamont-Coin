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
    
     private ClicksCountEventPair[] countEventPairs;
    private int goalIndex;
    private int currentGoal;
    private static GameEventsHandler instance;
    private void Start()
    {
        goalIndex = 0;
        //currentGoal = countEventPairs[goalIndex].ClicksCount;
        currentGoal = clickCountEventPair.Keys.ToArray()[goalIndex];

        Events.ClicksUpdated += CheckEnoughClicks;
    }
    
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
    public void CheckEnoughClicksOld()
    {
        if (goalIndex < countEventPairs.Length - 1)
        {
            if (GameManager.Clicks >= currentGoal)
            {
                //todo: optional delay on message send
                countEventPairs[goalIndex].Event.Invoke();
                goalIndex++;
                currentGoal = countEventPairs[goalIndex].ClicksCount;
            }
        }
        else
        {
            Debug.Log("All goals achieved");
        }
    } 
    public void CheckEnoughClicks()
    {
        if (goalIndex < clickCountEventPair.Count)
        {
            if (GameManager.Clicks >= currentGoal)
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
