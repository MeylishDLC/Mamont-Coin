using System;
using System.Numerics;
using Script.Data;
using Script.Managers;
using Script.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        public IDataBank DataBank { get; private set; }
        public AudioManager AudioManager { get; private set; }
        public FMODEvents FMODEvents { get; private set; }

        [Inject]
        public void Construct(IDataBank dataBank, AudioManager audioManager, FMODEvents fmodEvents)
        {
            DataBank = dataBank;
            AudioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        private void UpdateButtonInteractive(BigInteger _)
        {
            button.interactable = DataBank.Clicks >= price;
        }

        private void Start()
        {
            priceText.text = price.ToString();
            
            button.onClick.AddListener(BuyBuff);
            ClickHandler.ClicksUpdated += UpdateButtonInteractive; 
            UpdateButtonInteractive(0); 
        }

        public virtual void BuyBuff() { }
        private void OnDestroy()
        {
            ClickHandler.ClicksUpdated -= UpdateButtonInteractive;
        }
    }
}