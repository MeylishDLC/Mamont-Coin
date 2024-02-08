using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NotepadInteractable : MonoBehaviour, IWindowedApp
{
    [Header("Open/Close Window")]
    [SerializeField] private Button closeButton;
    [SerializeField] private float scaleOnOpen;
    [SerializeField] private float scaleOnClose;
    [SerializeField] private float openDuration;
    [SerializeField] private bool closedOnStart;

    [Header("Boost Options")] 
    [SerializeField] private GameObject boostTextPrefab;
    [SerializeField] private NotepadChoices boostChoices;
    [SerializeField] private TextMeshProUGUI boostText;
    private TextMeshProUGUI boostDescriptionText;

    private void Start()
    {
        if (closedOnStart)
        {
            gameObject.SetActive(false);
        }
        boostDescriptionText = boostTextPrefab.GetComponentInChildren<TextMeshProUGUI>();
        
        boostText.text = " ";
        boostDescriptionText.text = " ";
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
