using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace Script.Core
{
    public class ProgressHandler : MonoBehaviour
    {
        [Header("Ranks")] 
        [SerializeField] private SerializedDictionary<string, long> rankNameGoalPair;
    
        [Header("Mamont Title")] 
        [SerializeField] private TMP_Text mamontTitle;
        [SerializeField] private float mamontTitleScale;
    
        [Header("Progress Bar")]
        [SerializeField] private Slider progressBar;
        private float currentValue;
        private float maxValue;
        private int currentGoalIndex;

        private void Start()
        {
            currentValue = DataBank.Clicks;
            currentGoalIndex = 1;

            maxValue = rankNameGoalPair.Values.ElementAt(currentGoalIndex);
            mamontTitle.text = rankNameGoalPair.Keys.First();
        
            ClickHandler.ClicksUpdated += UpdateProgress;
        }

        private void OnDestroy()
        {
            ClickHandler.ClicksUpdated -= UpdateProgress;
        }

        private void UpdateProgress(int addAmount)
        {
            if (progressBar.value < maxValue)
            {
                progressBar.value = currentValue / maxValue;
            }
       
            if (currentValue == maxValue)
            {
                if (currentGoalIndex < rankNameGoalPair.Count)
                {
                    UpdateMamontTitle(rankNameGoalPair.Keys.ElementAt(currentGoalIndex)).Forget();
                    currentGoalIndex++;
                    if (currentGoalIndex < rankNameGoalPair.Count)
                    {
                        maxValue = rankNameGoalPair.Values.ElementAt(currentGoalIndex);
                    }
                }
                else
                {
                    Debug.Log("All goals are finished");
                }
            }
        }
    
        private async UniTask UpdateMamontTitle(string titleName)
        {
            mamontTitle.text = titleName;

            var mamontObject = mamontTitle.gameObject;
            await mamontObject.transform.DOScale(mamontTitleScale, 0.2f).SetLoops(2, LoopType.Yoyo);
            mamontObject.transform.localScale = new Vector3(1,1,1);
        }
    
        public void AddProgress(long amount)
        {
            currentValue += amount;
        
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }
    }
}
