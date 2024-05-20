using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core.Boosts;
using Script.Data;
using Script.Managers;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Apps.NotePadScript
{
    public class NotepadWithChoices : MonoBehaviour, IWindowedApp
    {
        public BoostsManager BoostsManager { get; private set; }
        
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

        private int currentAct;

        private void Start()
        {
            BoostsManager = new BoostsManager(HackerChoiceBoost.Values.Concat(ScammerChoiceBoost.Values).Distinct().ToList());
            
            currentAct = 0;
            hackerChoiceText = hackerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            scammerChoiceText = scammerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            
            UpdateChoices();
            gameObject.SetActive(false);
            
            hackerChoiceButton.onClick.AddListener(()=> MakeChoice(Character.Hacker));
            scammerChoiceButton.onClick.AddListener(() => MakeChoice(Character.Scammer));
        }
        private void UpdateChoices()
        {
            if (currentAct == specificChoiceAct - 1)
            {
                hackerChoiceText.text = BoostsManager.SpecificBoostName;
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
                        BoostsManager.SpecificBoost();
                        
                        notepadInteractable.WriteDownNewBoost(BoostsManager.SpecificBoostName);
                    }
                    else
                    {
                        BoostsManager.EnableBoost(HackerChoiceBoost[hackerBoostName]);
                        
                        notepadInteractable.WriteDownNewBoost(hackerBoostName);
                    }
                    
                    break;
                
                case Character.Scammer:
                    
                    var scammerBoostName = ScammerChoiceBoost.Keys.ElementAt(currentAct);
                    
                    BoostsManager.EnableBoost(ScammerChoiceBoost[scammerBoostName]);
                    
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
            GameManager.GetInstance().interactionOff.SetActive(false);
        }

        public void OpenApp()
        {
            notepadInteractable.CloseApp();
            GameManager.GetInstance().interactionOff.SetActive(true);
            gameObject.SetActive(true);
            gameObject.transform.DOScale(zoomIn, 0.1f);
        }

        public void CloseApp()
        {
            CloseNotepadAsync().Forget();
        }
    }
}