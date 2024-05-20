using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Core
{
    public class AddClicksScript : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int clicksToAdd;
        [SerializeField] private int price;
        [Header("UI")]
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private TextMeshProUGUI clicksToAddText;
    
        private void Start()
        {
            priceText.text = price + " clicks";
            clicksToAddText.text = "ПРИБАВИТЬ " + clicksToAdd;
            UpdateButtonInteractable(clicksToAdd);

        }
        private void OnEnable()
        {
            button.onClick.AddListener(BuyClicks);
            ClickHandler.ClicksUpdated += UpdateButtonInteractable; 
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(BuyClicks); 
            ClickHandler.ClicksUpdated -= UpdateButtonInteractable; 
        }
    
        private void UpdateButtonInteractable(int _)
        {
            button.interactable = DataBank.Clicks >= price;
        }
        private void BuyClicks()
        {
            if (DataBank.Clicks >= price)
            {
                DataBank.Clicks -= price;
                
                var addAmount = clicksToAdd;
                DataBank.Clicks += addAmount;
                UpdateButtonInteractable(addAmount);
                
                ClickHandler.ClicksUpdated?.Invoke(addAmount);
            }
        }
    }
}
