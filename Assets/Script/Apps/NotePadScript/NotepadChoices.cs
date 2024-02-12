using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Data;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotepadChoices : MonoBehaviour, IWindowedApp
{
    [Header("Notepad Boost Choice")] [SerializeField]
    private GameObject notepad;
    [SerializeField] private float zoomIn;
    [SerializeField] private float zoomOut;

    [Header("Choices Set")] 
    [SerializeField] private Button hackerChoice;
    private TextMeshProUGUI hackerChoiceText;
    public List<ChoiceActionPair> hackerChoices;
    
    [SerializeField] private Button scammerChoice;
    private TextMeshProUGUI scammerChoiceText;
    public List<ChoiceActionPair> scammerChoices;
    
    [Header("Specific Choices")]
    [SerializeField] private int specificChoiceActNumber;

    [Header("Interactable Notepad")]
    [SerializeField] private NotepadInteractable notepadInteractable;

    public int CurrentAct { get; set; }

    private void Start()
    {
        CurrentAct = 0;
        hackerChoiceText = hackerChoice.GetComponentInChildren<TextMeshProUGUI>();
        scammerChoiceText = scammerChoice.GetComponentInChildren<TextMeshProUGUI>();
        UpdateChoices();
        notepad.SetActive(false);
    }

    private void UpdateChoices()
    {
        hackerChoiceText.text = CurrentAct != specificChoiceActNumber - 1 
            ? hackerChoices[CurrentAct].ChoiceName 
            : BoostsManager.CurrentSpecificBoostName;
        
        scammerChoiceText.text = scammerChoices[CurrentAct].ChoiceName;
    }

    public void MakeChoice(int choiceNum)
    {
        switch (choiceNum)
        {
            case 1:
                hackerChoices[CurrentAct].Event?.Invoke();
                notepadInteractable.InstantiateBoostInfo(CurrentAct != specificChoiceActNumber - 1
                    ? hackerChoices[CurrentAct].ChoiceName
                    : BoostsManager.CurrentSpecificBoostName);
                break;
            case 2:
                scammerChoices[CurrentAct].Event?.Invoke();
                notepadInteractable.InstantiateBoostInfo(scammerChoices[CurrentAct].ChoiceName);
                break;
            default:
                Debug.LogError("Wrong choice number, only 2 choices can exist");
                return;
        }

        if (CurrentAct < hackerChoices.Count - 1)
        {
            CurrentAct++;
        }
        else
        {
            Debug.Log("All boosts are enabled");
        }
        CloseNotepadAsync().Forget();
        UpdateChoices();
    }
    
    private async UniTask CloseNotepadAsync()
    {
        await notepad.transform.DOScale(zoomOut, 0.1f).ToUniTask();
        notepad.SetActive(false);
        GameManager.GetInstance().interactionOff.SetActive(false);
    }

    public void OpenApp()
    {
        notepadInteractable.CloseApp();
        GameManager.GetInstance().interactionOff.SetActive(true);
        notepad.SetActive(true);
        notepad.transform.DOScale(zoomIn, 0.1f);
    }

    public void CloseApp()
    {
        CloseNotepadAsync().Forget();
    }
}
