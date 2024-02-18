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
    
    private void UpdateButtonInteractable()
    {
        button.interactable = GameManager.Clicks >= price;
    }

    private void Start()
    {
        priceText.text = price.ToString();
        
        Events.ClicksUpdated += UpdateButtonInteractable; 
        UpdateButtonInteractable(); 
    }

    private async UniTask BuyTimesMultiplierTimeLimitedAsync()
    { 
        BuyTimesMultiplierBuff(); 
        await UniTask.Delay(timeLimitMilliseconds);
        GameManager.Multiplier /= multiplier;
    }

    public void BuyTimesMultiplierTimeLimited()
    {
        BuyTimesMultiplierTimeLimitedAsync().Forget();
    }
    
    //todo: time limited add multiplier
    public void BuyAddMultiplier()
    {
        if (GameManager.Clicks >= price)
        {
            GameManager.Multiplier += multiplier;
            GameManager.Clicks -= price;
            
            Events.ClicksUpdated?.Invoke();
        }
    }

    public void BuyTimesMultiplierBuff()
    {
        if (GameManager.Clicks >= price)
        {
            GameManager.Multiplier *= multiplier;
            GameManager.Clicks -= price;
            
            Events.ClicksUpdated?.Invoke();
        }
    }
}
