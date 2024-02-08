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
    private void Start()
    {
        counterText.text = GameManager.Clicks.ToString();
        multiplierText.text = GameManager.Multiplier + " кликов";
        coinToRubleText.text = "0 руб.";

        Events.ClicksUpdated += OnClicksUpdated;
        Events.DoubleClickChanceEnabled += AddDoubleClick;
    }
    
    public void AddDoubleClick()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(TaskBackgroundManager.GetInstance().DoubleClickChance);
    }
    public void Increment()
    {
        GameManager.Clicks += GameManager.Multiplier;
        Events.ClicksUpdated?.Invoke();
        GameEventsHandler.GetInstance().CheckEnoughClicks();
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
