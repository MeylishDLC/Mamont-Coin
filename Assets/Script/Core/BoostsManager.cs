using System;
using Cysharp.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoostsManager : MonoBehaviour
{
    private static BoostsManager instance;
    public bool autoClickerEnabled;
    public bool paidPopupWindowEnabled;
    public static bool DoubleClickChanceEnabled;

    [Header("Specific boost setting")]
    [SerializeField] private int autoClickAmountImprove;
    [SerializeField] private int doubleClickImprove;
    
    [SerializeField] private string doubleClickImproveText;
    [SerializeField] private string autoClickImproveText;
    public static string CurrentSpecificBoostName;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one BoostManager in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        TaskBackgroundManager.GetInstance().PopupWindowAppearAsync().Forget();
    }

    public static BoostsManager GetInstance()
    {
        return instance;
    }

    public void AutoClicker()
    {
        autoClickerEnabled = true;
        TaskBackgroundManager.GetInstance().AutoClick();
        CurrentSpecificBoostName = autoClickImproveText;
    }

    public void DownloadAmegas()
    {
        paidPopupWindowEnabled = true;
        TaskBackgroundManager.GetInstance().PaidPopupWindowAppear();
    }

    public void Refelalka(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }

    public void DoubleClick()
    {
        DoubleClickChanceEnabled = true;
        CurrentSpecificBoostName = doubleClickImproveText;
    }
    
    public void SpecificBoost()
    {
        if (autoClickerEnabled)
        {
            TaskBackgroundManager.autoClickAmount = autoClickAmountImprove;
        }
        else if (DoubleClickChanceEnabled)
        {
            TaskBackgroundManager.doubleClickAmount = doubleClickImprove;
        }
    }
    
    public void MoneyBonus(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }
    
    
}
