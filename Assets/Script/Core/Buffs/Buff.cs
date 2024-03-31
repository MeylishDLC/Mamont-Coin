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
        [SerializeField] protected TMP_Text multiplierText;
        
        protected void UpdateButtonInteractive()
        {
            button.interactable = GameManager.Clicks >= price;
        }

        private void Start()
        {
            priceText.text = price.ToString();
        
            Events.ClicksUpdated += UpdateButtonInteractive; 
            UpdateButtonInteractive(); 
        }
    }
}