using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core;
using Script.Data;
using Script.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Script.Apps.NotePadScript
{
    public class NotepadWithChoices : MonoBehaviour, IWindowedApp
    {
        [SerializeField] private float zoomIn;
        [SerializeField] private float zoomOut;

        [Header("Choices Set")] 
        [SerializeField] private Button hackerChoiceButton;
        private TextMeshProUGUI hackerChoiceText;
        public SerializedDictionary<string, UnityEvent> HackerChoiceAction;

        [SerializeField] private Button scammerChoiceButton;
        private TextMeshProUGUI scammerChoiceText;
        public SerializedDictionary<string, UnityEvent> ScammerChoiceAction;

        [Header("Specific Choices")] 
        [SerializeField] private int specificChoiceAct;

        [Header("Interactable Notepad")] 
        [SerializeField] private NotepadInteractable notepadInteractable;

        public int CurrentAct { get; set; }

        private void Start()
        {
            CurrentAct = 0;
            hackerChoiceText = hackerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            scammerChoiceText = scammerChoiceButton.GetComponentInChildren<TextMeshProUGUI>();
            
            UpdateChoices();
            gameObject.SetActive(false);
            
            hackerChoiceButton.onClick.AddListener(()=> MakeChoice(Character.Hacker));
            scammerChoiceButton.onClick.AddListener(() => MakeChoice(Character.Scammer));
        }

        //  UPDATE CHOICES - sets the name of next choice after the choice was made
        private void UpdateChoices()
        {
            if (CurrentAct == specificChoiceAct - 1)
            {
                hackerChoiceText.text = BoostsManager.CurrentSpecificBoostName;
            }
            else
            {
                hackerChoiceText.text = HackerChoiceAction.Keys.ElementAt(CurrentAct);
            }

            scammerChoiceText.text = ScammerChoiceAction.Keys.ElementAt(CurrentAct);
        }

        private void MakeChoice(Character characterChoice)
        {
            switch (characterChoice)
            {
                case Character.Hacker:

                    var hackerBoostName = HackerChoiceAction.Keys.ElementAt(CurrentAct);
                    HackerChoiceAction[hackerBoostName]?.Invoke();
                    if (CurrentAct == specificChoiceAct - 1)
                    {
                        notepadInteractable.WriteDownNewBoost(BoostsManager.CurrentSpecificBoostName);
                    }
                    else
                    {
                        notepadInteractable.WriteDownNewBoost(hackerBoostName);
                    }
                    
                    break;
                
                case Character.Scammer:
                    
                    var scammerBoostName = ScammerChoiceAction.Keys.ElementAt(CurrentAct);
                    ScammerChoiceAction[scammerBoostName]?.Invoke();
                    notepadInteractable.WriteDownNewBoost(scammerBoostName);
                    
                    break;
                
                default:
                    return;
            }

            if (CurrentAct < HackerChoiceAction.Count - 1)
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