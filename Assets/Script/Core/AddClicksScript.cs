using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
        UpdateButtonInteractable();

    }
    private void OnEnable()
    {
        button.onClick.AddListener(BuyClicks);
        Events.ClicksUpdated += UpdateButtonInteractable; 
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(BuyClicks); 
        Events.ClicksUpdated -= UpdateButtonInteractable; 
    }
    
    private void UpdateButtonInteractable()
    {
        button.interactable = DataBank.Clicks >= price;
    }
    private void BuyClicks()
    {
        if (DataBank.Clicks >= price)
        {
            DataBank.Clicks -= price;
            DataBank.Clicks += clicksToAdd;
            UpdateButtonInteractable();
            Events.ClicksUpdated?.Invoke();
        }
    }
}
