using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClickHandler : MonoBehaviour
{ 
    //todo: clicker animations
    [SerializeField] private TextMeshProUGUI counterText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private TextMeshProUGUI coinToRubleText;
    [SerializeField] private int coinExchangeRate;

    [SerializeField] private ProgressHandler progressHandler;
    private void Start()
    {
        counterText.text = GameManager.Clicks.ToString();
        multiplierText.text = GameManager.Multiplier + " кликов";
        coinToRubleText.text = "0 руб.";

        Events.ClicksUpdated += OnClicksUpdated;
    }
    
    public void Increment()
    {
        if (BoostsManager.GetInstance().doubleClick.IsEnabled)
        {
            var doubleClick = BoostsManager.GetInstance().doubleClick;
            GameManager.Clicks += GameManager.Multiplier * doubleClick.DoubleClickChance();
            progressHandler.AddProgress(GameManager.Multiplier * doubleClick.DoubleClickChance());
            
            Events.ClicksUpdated?.Invoke();
        }
        else
        {
            GameManager.Clicks += GameManager.Multiplier;
            progressHandler.AddProgress(GameManager.Multiplier);
            Events.ClicksUpdated?.Invoke();
        }
    }

    private void OnClicksUpdated()
    {
        counterText.text = GameManager.Clicks.ToString();
        multiplierText.text = GameManager.Multiplier + " кликов";

        var clicks = GameManager.Clicks;
        var rubles = Convert.ToDouble(clicks) / Convert.ToDouble(coinExchangeRate);
        coinToRubleText.text = rubles + " руб.";
    }

    
}
