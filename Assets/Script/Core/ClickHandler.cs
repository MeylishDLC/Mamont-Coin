using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI multiplierText;

    private void Start()
    {
        counterText.text = "CLICKS: " + GameManager.Clicks;
        multiplierText.text = "MULTIPLIER: " + GameManager.Multiplier;
        Events.ClicksUpdated += OnClicksUpdated;
    }
    public void Increment()
    {
        GameManager.Clicks += GameManager.Multiplier;
        Events.ClicksUpdated?.Invoke();
        GameEventsHandler.GetInstance().CheckEnoughClicks();
    }

    private void OnClicksUpdated()
    {
        counterText.text = "CLICKS: " + GameManager.Clicks;
        multiplierText.text = "MULTIPLIER: " + GameManager.Multiplier;
    }

    
}
