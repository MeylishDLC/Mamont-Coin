using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NotepadInteractable : MonoBehaviour, IWindowedApp
{
    [Header("Open/Close Window")]
    [SerializeField] private Button closeButton;
    [SerializeField] private float scaleOnOpen;
    [SerializeField] private float scaleOnClose;
    [SerializeField] private float openDuration;
    
    [Header("Boosts chosen")] 
    [SerializeField] private List<TMP_Text> chosenBoosts;
    private int chosenBoostAmount;
    private void Start()
    {
        gameObject.SetActive(false);

        foreach (var boostInfo in chosenBoosts)
        {
            boostInfo.gameObject.SetActive(false);
        }
        
        closeButton.onClick.AddListener(CloseApp);
    }

    public void WriteDownNewBoost(string boostName)
    {
        chosenBoostAmount++;
        chosenBoosts[chosenBoostAmount - 1].text = boostName;
        chosenBoosts[chosenBoostAmount - 1].gameObject.SetActive(true);
    }
    
    
    public void OpenApp()
    {
        OpenAppAsync().Forget();
    }
    
    public void CloseApp()
    {
        CloseAppAsync().Forget();
    }
    
    private async UniTask OpenAppAsync()
    {
        gameObject.SetActive(true);
        closeButton.interactable = false;
        
        await transform.DOScale(scaleOnOpen, openDuration).ToUniTask();

        closeButton.interactable = true;
    }
    
    private async UniTask CloseAppAsync()
    {
        closeButton.interactable = false;
        
        await transform.DOScale(scaleOnClose, openDuration).ToUniTask();

        closeButton.interactable = true;
        gameObject.SetActive(false);
    }
}
