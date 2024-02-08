using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierBuff : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int multiplier;
    [SerializeField] private int price;
    [SerializeField] private int timeLimitMilliseconds;
    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI multiplierText;

    private void OnEnable()
    {
        if (timeLimitMilliseconds > 0)
        {
            button.onClick.AddListener(BuyMultiplierBuffTimeLimited);
        }
        else
        {
            button.onClick.AddListener(BuyMultiplierBuff);
        }
        Events.ClicksUpdated += UpdateButtonInteractable; 
    }

    private void OnDisable()
    {
        if (timeLimitMilliseconds > 0)
        {
            button.onClick.RemoveListener(BuyMultiplierBuff); 
        }
        else
        {
            button.onClick.RemoveListener(BuyMultiplierBuffTimeLimited);
        }
        Events.ClicksUpdated -= UpdateButtonInteractable; 
    }

    private void UpdateButtonInteractable()
    {
        button.interactable = GameManager.Clicks >= price;
    }

    private void Start()
    {
        priceText.text = price.ToString();
        //multiplierText.text = "+" + multiplier + " КЛИКОВ";
        UpdateButtonInteractable(); 
    }

    private async UniTask BuyMultiplierBuffTimeLimitedAsync()
    {
        if (GameManager.Clicks >= price)
        {
            GameManager.Multiplier += multiplier;
            GameManager.Clicks -= price;
            UpdateButtonInteractable();
            Events.ClicksUpdated?.Invoke();
            await UniTask.Delay(timeLimitMilliseconds);
            GameManager.Multiplier -= multiplier;
        }
    }

    public void BuyMultiplierBuffTimeLimited()
    {
        BuyMultiplierBuffTimeLimitedAsync().Forget();
    }
    public void BuyMultiplierBuff()
    {
        if (GameManager.Clicks >= price)
        {
            GameManager.Multiplier += multiplier;
            GameManager.Clicks -= price;
            UpdateButtonInteractable();
            Events.ClicksUpdated?.Invoke();
        }
    }
}
