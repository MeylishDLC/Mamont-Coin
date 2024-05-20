using System;
using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Core.Buffs
{
    public class Buff : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] protected int buffAmount;
        [SerializeField] protected int price;
        [Header("UI")]
        [SerializeField] protected Button button;
        [SerializeField] protected TMP_Text priceText;
        
        private void UpdateButtonInteractive(int _)
        {
            button.interactable = DataBank.Clicks >= price;
        }

        private void Start()
        {
            priceText.text = price.ToString();
        
            ClickHandler.ClicksUpdated += UpdateButtonInteractive; 
            UpdateButtonInteractive(0); 
        }

        private void OnDestroy()
        {
            ClickHandler.ClicksUpdated -= UpdateButtonInteractive;
        }
    }
}