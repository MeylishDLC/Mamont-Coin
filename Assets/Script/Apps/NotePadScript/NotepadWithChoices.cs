﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core.Boosts;
using Script.Data;
using Script.Managers;
using Script.Sound;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace Script.Apps.NotePadScript
{
    public class NotepadWithChoices : MonoBehaviour, IWindowedApp
    {
        [SerializeField] private float zoomIn;
        [SerializeField] private float zoomOut;

        [Header("Choices Set")] 
        [SerializeField] private Button hackerChoiceButton;
        private TextMeshProUGUI hackerChoiceText;
        
        public SerializedDictionary<string, Boost> HackerChoiceBoost;

        [SerializeField] private Button scammerChoiceButton;
        private TextMeshProUGUI scammerChoiceText;
        
        public SerializedDictionary<string, Boost> ScammerChoiceBoost;

        [Header("Specific Choices")] 
        [SerializeField] private int specificChoiceAct;

        [Header("Interactable Notepad")] 
        [SerializeField] private NotepadInteractable notepadInteractable;
        [SerializeField] private GameObject interactionOff;
        
        private int currentAct;
        private BoostsService boostsService;
        
        private AudioManager audioManager;
        private FMODEvents FMODEvents;

        [Inject]
        public void Construct(BoostsService boostsService, AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.boostsService = boostsService;
            this.boostsService.BoostsToManage = HackerChoiceBoost.Values.Concat(ScammerChoiceBoost.Values).Distinct().ToList();

            this.audioManager = audioManager;
            FMODEvents = fmodEvents;

            this.interactionOff = interactionOff;
        }
        
        private void Start()
        {
            currentAct = 0;
            hackerChoiceText = hackerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            scammerChoiceText = scammerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            
            UpdateChoices();
            gameObject.SetActive(false);
            
            hackerChoiceButton.onClick.AddListener(()=> MakeChoice(Character.Hacker));
            scammerChoiceButton.onClick.AddListener(() => MakeChoice(Character.Scammer));
        }

        private void OnDestroy()
        {
            hackerChoiceButton.onClick.RemoveAllListeners();
            scammerChoiceButton.onClick.RemoveAllListeners();
        }

        private void UpdateChoices()
        {
            if (currentAct == specificChoiceAct - 1)
            {
                hackerChoiceText.text = boostsService.SpecificBoostName;
            }
            else
            {
                hackerChoiceText.text = HackerChoiceBoost.Keys.ElementAt(currentAct);
            }

            scammerChoiceText.text = ScammerChoiceBoost.Keys.ElementAt(currentAct);
        }

        private void MakeChoice(Character characterChoice)
        {
            switch (characterChoice)
            {
                case Character.Hacker:
                    var hackerBoostName = HackerChoiceBoost.Keys.ElementAt(currentAct);
                    if (currentAct == specificChoiceAct - 1)
                    {
                        boostsService.SpecificBoost();
                        notepadInteractable.WriteDownNewBoost(boostsService.SpecificBoostName);
                    }
                    else
                    {
                        boostsService.EnableBoost(HackerChoiceBoost[hackerBoostName]);
                        notepadInteractable.WriteDownNewBoost(hackerBoostName);
                    }
                    break;
                
                case Character.Scammer:
                    var scammerBoostName = ScammerChoiceBoost.Keys.ElementAt(currentAct);
                    boostsService.EnableBoost(ScammerChoiceBoost[scammerBoostName]);
                    notepadInteractable.WriteDownNewBoost(scammerBoostName);
                    break;
                
                default:
                    return;
            }

            if (currentAct < HackerChoiceBoost.Count - 1)
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
            await gameObject.transform.DOScale(zoomOut, 0.1f).ToUniTask();
            gameObject.SetActive(false);
            interactionOff.SetActive(false);
        }
        public void OpenApp()
        {
            audioManager.PlayOneShot(FMODEvents.boostChoiceSound);
            notepadInteractable.CloseApp();
            interactionOff.SetActive(true);
            gameObject.SetActive(true);
            gameObject.transform.DOScale(zoomIn, 0.1f);
        }
        public void CloseApp()
        {
            CloseNotepadAsync().Forget();
        }
    }
}