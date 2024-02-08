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
    [SerializeField] private NotepadChoices notepad;
    [SerializeField] private GameObject notepadContainer;
    [SerializeField] private GameObject boostPrefab;
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

    private void InstantiateBoostInfo(bool isFirstChoice = true)
    {
        var boostInfo = Instantiate(boostPrefab, notepadContainer.transform);
        var boostInfoText = boostPrefab.GetComponentInChildren<TextMeshProUGUI>();
        
        boostInfoText.SetText(isFirstChoice
            ? notepad.firstChoices[notepad.currentAct].ChoiceName
            : notepad.secondChoices[notepad.currentAct].ChoiceName);
    }
    
    public void AutoClicker()
    {
        InstantiateBoostInfo(true);
        autoClickerEnabled = true;
        TaskBackgroundManager.GetInstance().AutoClick(TaskBackgroundManager.GetInstance().autoclickAmount);
    }

    public void DownloadAmegas()
    {
        InstantiateBoostInfo(true);
        autoPopupWindowEnabled = true;
        TaskBackgroundManager.GetInstance().PopupWindowAppear();
    }

    public void Refelalka(int bonus)
    {
        InstantiateBoostInfo(false);
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }
    
    public void DoubleClickChance()
    {
        InstantiateBoostInfo(false);
        doubleClickChanceEnabled = true;
    }

    public void ImproveAutoClick()
    {
        
    }

    public void MoneyBonus(int bonus)
    {
        InstantiateBoostInfo();
        GameManager.Clicks += bonus;
        Events.ClicksUpdated?.Invoke();
    }

    public void ImproveDoubleClickChance(int improvedClickAmount)
    {
        
    }
    
}
