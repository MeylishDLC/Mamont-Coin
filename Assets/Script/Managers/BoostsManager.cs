using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Core;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class BoostsManager : MonoBehaviour
{
    [Header("Auto Clicker")] 
    [SerializeField] private int clickFrequencyMilliseconds;
    [SerializeField] private int autoClickAmount = 1;
    private AutoClicker autoClicker;
        
    [Header("Double Click Chance")]
    [SerializeField] private int percentageChanceOfDoubleClick;
    public int doubleClickAmount = 2;
    public DoubleClick doubleClick;

   
    [Header("Paid Pop-up Windows")]
    [SerializeField] private int paidAppearFrequencyMilliseconds;
    public int coinsPerPopupWindow;
    private PaidPopups paidPopups;
    
    [Header("Specific boost setting")]
    [SerializeField] private int autoClickAmountImprove;
    [SerializeField] private string autoClickImproveText;
    
    [SerializeField] private int doubleClickImprove;
    [SerializeField] private string doubleClickImproveText;

    public static string CurrentSpecificBoostName;
    private List<Boost> boosts;

    #region Set Instance
    
    private static BoostsManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one BoostManager in the scene.");
        }
        instance = this;
    }
    public static BoostsManager GetInstance()
    {
        return instance;
    }
    
    #endregion
    private void Start()
    {
        autoClicker = new AutoClicker(clickFrequencyMilliseconds, autoClickAmount);
        doubleClick = new DoubleClick(percentageChanceOfDoubleClick, doubleClickAmount);
        paidPopups = new PaidPopups(paidAppearFrequencyMilliseconds, PopupsManager.GetInstance().popupWindows);

        boosts = new List<Boost>
        {
            autoClicker,
            doubleClick,
            paidPopups
        };
    }

    public void EnableAutoClicker()
    {
        autoClicker.Activate();
        CurrentSpecificBoostName = autoClickImproveText;
    }
    
    public void EnableDoubleClick()
    {
        doubleClick.Activate();
        CurrentSpecificBoostName = doubleClickImproveText;
    }
    
    public void DownloadAmegas()
    {
        paidPopups.Activate();
    }
    public void Refelalka(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }
    public void MoneyBonus(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }
    public void DisableAllBoosts()
    {
        foreach (var boost in boosts)
        {
            boost.IsEnabled = false;
        }
    }
    
    public void SpecificBoost()
    {
        if (autoClicker.IsEnabled)
        {
            autoClicker.AutoClickAmount = autoClickAmountImprove;
        }
        else if (doubleClick.IsEnabled)
        {
            doubleClick.DoubleClickAmount = doubleClickImprove;
        }
    }
    
    
    
}
