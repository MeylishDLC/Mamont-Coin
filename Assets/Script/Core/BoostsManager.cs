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
        interactableNotepad.InstantiateBoostInfo(true);
        autoClickerEnabled = true;
        TaskBackgroundManager.GetInstance().AutoClick(TaskBackgroundManager.GetInstance().autoclickAmount);
    }

    public void DownloadAmegas()
    {
        interactableNotepad.InstantiateBoostInfo(true);
        autoPopupWindowEnabled = true;
        TaskBackgroundManager.GetInstance().PopupWindowAppear();
    }

    public void Refelalka(int bonus)
    {
        interactableNotepad.InstantiateBoostInfo(false);
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }
    
    public void DoubleClickChance()
    {
        interactableNotepad.InstantiateBoostInfo(false);
        doubleClickChanceEnabled = true;
    }

    public void ImproveAutoClick()
    {
        
    }

    public void MoneyBonus(int bonus)
    {
        interactableNotepad.InstantiateBoostInfo();
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }

    public void ImproveDoubleClickChance(int improvedClickAmount)
    {
        
    }
    
}
