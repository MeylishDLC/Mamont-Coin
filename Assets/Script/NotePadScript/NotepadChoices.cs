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
    [SerializeField] private Button firstChoice;
    private TextMeshProUGUI firstChoiceText;
    public List<ChoiceActionPair> firstChoices;
    
    [SerializeField] private Button secondChoice;
    private TextMeshProUGUI secondChoiceText;
    public List<ChoiceActionPair> secondChoices;
    
    public int currentAct;

    private void Start()
    {
        currentAct = 0;
        firstChoiceText = firstChoice.GetComponentInChildren<TextMeshProUGUI>();
        secondChoiceText = secondChoice.GetComponentInChildren<TextMeshProUGUI>();
        UpdateChoices();
        notepad.SetActive(false);
    }

    private void UpdateChoices()
    {
        firstChoiceText.text = firstChoices[currentAct].ChoiceName;
        secondChoiceText.text = secondChoices[currentAct].ChoiceName;
    }

    public void MakeChoice(int choiceNum)
    {
        switch (choiceNum)
        {
            case 1:
                firstChoices[currentAct].Event?.Invoke();
                break;
            case 2:
                secondChoices[currentAct].Event?.Invoke();
                break;
            default:
                Debug.LogError("Wrong choice number, only 2 choices can exist");
                return;
        }

        if (currentAct < firstChoices.Count - 1)
        {
            currentAct++;
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
        GameManager.GetInstance().interactionOff.SetActive(true);
        notepad.SetActive(true);
        notepad.transform.DOScale(zoomIn, 0.1f);
    }

    public void CloseApp()
    {
        CloseNotepadAsync().Forget();
    }
}
