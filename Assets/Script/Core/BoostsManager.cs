using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BoostsManager : MonoBehaviour
{
    private static BoostsManager instance;
    public bool autoClickerEnabled;
    public bool autoPopupWindowEnabled;
    public bool doubleClickChanceEnabled;
    
    [SerializeField] private NotepadInteractable interactableNotepad;
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

    public void AutoClicker()
    {
        autoClickerEnabled = true;
        TaskBackgroundManager.GetInstance().AutoClick(TaskBackgroundManager.GetInstance().autoClickAmount);
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
        doubleClickChanceEnabled = true;
    }

    public void ImproveAutoClick()
    {
        
    }

    public void MoneyBonus(int bonus)
    {
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }

    public void ImproveDoubleClickChance(int improvedClickAmount)
    {
        
    }
    
}
