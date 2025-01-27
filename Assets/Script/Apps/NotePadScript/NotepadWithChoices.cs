﻿using System.Linq;
using System.Numerics;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core;
using Script.Core.Boosts;
using Script.Data;
using Script.Managers;
using Script.Sound;
using Script.UI;
using TMPro;
using UnityEngine;
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
        private SpecificBoostSetter specificBoostSetter;
        private IDataBank dataBank;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;

        [Inject]
        public void Construct(IDataBank dataBank, SpecificBoostSetter specificBoostSetter, AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.dataBank = dataBank;
            this.specificBoostSetter = specificBoostSetter;
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        
        private void Start()
        {
            currentAct = 0;
            hackerChoiceText = hackerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            scammerChoiceText = scammerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            
            UpdateChoices();
            gameObject.SetActive(false);
            
            hackerChoiceButton.onClick.AddListener(()=> MakeChoice(ChatCharacter.Hacker));
            scammerChoiceButton.onClick.AddListener(() => MakeChoice(ChatCharacter.Scammer));

            Boost.OnBoostAddClicks += AddClicksFromBoosts;
        }
        private void AddClicksFromBoosts(BigInteger amount)
        {
            dataBank.Clicks += amount;
            ClickHandler.ClicksUpdated.Invoke(amount);
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
                hackerChoiceText.text = specificBoostSetter.SpecificBoostName;
            }
            else
            {
                hackerChoiceText.text = HackerChoiceBoost.Keys.ElementAt(currentAct);
            }

            scammerChoiceText.text = ScammerChoiceBoost.Keys.ElementAt(currentAct);
        }

        private void MakeChoice(ChatCharacter chatCharacterChoice)
        {
            switch (chatCharacterChoice)
            {
                case ChatCharacter.Hacker:
                    var hackerBoostName = HackerChoiceBoost.Keys.ElementAt(currentAct);
                    if (currentAct == specificChoiceAct - 1)
                    {
                        specificBoostSetter.SpecificBoost();
                        notepadInteractable.WriteDownNewBoost(specificBoostSetter.SpecificBoostName);
                    }
                    else
                    {
                        specificBoostSetter.EnableBoost(HackerChoiceBoost[hackerBoostName]);
                        notepadInteractable.WriteDownNewBoost(hackerBoostName);
                    }
                    break;
                
                case ChatCharacter.Scammer:
                    var scammerBoostName = ScammerChoiceBoost.Keys.ElementAt(currentAct);
                    specificBoostSetter.EnableBoost(ScammerChoiceBoost[scammerBoostName]);
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