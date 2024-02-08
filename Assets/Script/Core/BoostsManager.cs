using System;
using UnityEngine;

public class BoostsManager : MonoBehaviour
{
    private static BoostsManager instance;
    public bool autoClickerEnabled;
    public bool autoPopupWindowEnabled;
    public bool doubleClickChanceEnabled;
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
        
    }

    public static BoostsManager GetInstance()
    {
        return instance;
    }
    
    public void AutoClicker()
    {
        autoClickerEnabled = true;
        TaskBackgroundManager.GetInstance().AutoClick(TaskBackgroundManager.GetInstance().autoclickAmount);
    }

    public void DownloadAmegas()
    {
        autoPopupWindowEnabled = true;
        TaskBackgroundManager.GetInstance().PopupWindowAppear();
    }

    public void Refelalka(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }

    public void DoubleClickChance()
    {
        //todo: check double click
        doubleClickChanceEnabled = true;
        TaskBackgroundManager.GetInstance().DoubleClickChance();
    }

    public void ImproveAutoClick(int improvedClickAmount)
    {
        if (autoClickerEnabled)
        {
            TaskBackgroundManager.GetInstance().autoclickAmount = improvedClickAmount;
            TaskBackgroundManager.GetInstance().AutoClick(improvedClickAmount);
        }
        else
        {
            Debug.Log("Could not improve autoclick since it's not enabled");
        }
    }

    public void MoneyBonus(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }

    public void ImproveDoubleClickChance(int improvedClickAmount)
    {
        if (doubleClickChanceEnabled)
        {
            TaskBackgroundManager.GetInstance().chanceClickAmount = improvedClickAmount;
            TaskBackgroundManager.GetInstance().DoubleClickChance(improvedClickAmount);
        }
        else
        {
            Debug.Log("Could not improve double click since it's not enabled");
        }
    }
    
}
