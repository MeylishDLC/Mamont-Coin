using System;
using System.Collections;
using System.Collections.Generic;
using Script.Data;
using UnityEngine;
using UnityEngine.Events;

public class GameEventsHandler : MonoBehaviour
{
    [SerializeField] private ClicksCountEventPair[] countEventPairs;
    private int goalIndex;
    private int currentGoal;
    private static GameEventsHandler instance;
    private void Start()
    {
        goalIndex = 0;
        currentGoal = countEventPairs[goalIndex].ClicksCount;
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
    public void CheckEnoughClicks()
    {
        if (goalIndex < countEventPairs.Length - 1)
        {
            if (GameManager.Clicks >= currentGoal)
            {
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
    
}
