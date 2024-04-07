using System;
using System.Collections;
using System.Collections.Generic;
using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{ 
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI coinToRubleText;
    [SerializeField] private int coinExchangeRate;
    [SerializeField] private ProgressHandler progressHandler;
    private void Start()
    {
        counterText.text = DataBank.Clicks.ToString();
        multiplierText.text = DataBank.Multiplier + " кликов";
        coinToRubleText.text = "0 руб.";

        Events.ClicksUpdated += OnClicksUpdated;
    }
    
    public void Increment()
    {
        if (BoostsManager.GetInstance().doubleClick.IsEnabled)
        {
            var doubleClick = BoostsManager.GetInstance().doubleClick;
            DataBank.Clicks += DataBank.Multiplier * doubleClick.DoubleClickChance();
            progressHandler.AddProgress(DataBank.Multiplier * doubleClick.DoubleClickChance());
            
            Events.ClicksUpdated?.Invoke();
        }
        else
        {
            DataBank.Clicks += DataBank.Multiplier;
            progressHandler.AddProgress(DataBank.Multiplier);
            Events.ClicksUpdated?.Invoke();
        }
    }

    private void OnClicksUpdated()
    {
        counterText.text = DataBank.Clicks.ToString();
        multiplierText.text = DataBank.Multiplier + " кликов";
        
        var clicks = DataBank.Clicks;
        var rubles = Convert.ToDouble(clicks) / Convert.ToDouble(coinExchangeRate);
        coinToRubleText.text = rubles + " руб.";
    }

    
}
