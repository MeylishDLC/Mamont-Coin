using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Data;
using Script.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Vector3 = UnityEngine.Vector3;

namespace Script.Core
{
    public class ProgressHandler : MonoBehaviour
    {
        [Header("Ranks")] 
        [SerializeField] private SerializedDictionary<RankNamePair, long> rankNameGoalPair;
    
        [Header("Mamont Title")] 
        [SerializeField] private TMP_Text mamontTitle;
        [SerializeField] private float mamontTitleScale;
    
        [Header("Progress Bar")]
        [SerializeField] private Slider progressBar;
        
        private float currentValue;
        private float maxValue;
        private int currentGoalIndex;

        private IDataBank dataBank;
        
        public static event Action<string, Sprite> OnNewMamontTitleReached;

        [Inject]
        public void Construct(IDataBank dataBank)
        {
            this.dataBank = dataBank;
        }
        private void Start()
        {
            currentValue = dataBank.Clicks;
            currentGoalIndex = 1;

            maxValue = rankNameGoalPair.Values.ElementAt(currentGoalIndex);
            mamontTitle.text = rankNameGoalPair.Keys.First().Name;

            OnNewMamontTitleReached += UpdateMamontTitle;
            ClickHandler.ClicksUpdated += UpdateProgress;
        }

        private void OnDestroy()
        {
            ClickHandler.ClicksUpdated -= UpdateProgress;
            OnNewMamontTitleReached -= UpdateMamontTitle;
        }

        private void UpdateProgress(int addAmount)
        {
            if (addAmount > 0)
            {
                AddProgress(addAmount);
            }
            
            if (progressBar.value < maxValue)
            {
                progressBar.value = currentValue / maxValue;
            }
       
            if (currentValue == maxValue)
            {
                if (currentGoalIndex < rankNameGoalPair.Count)
                {
                    var pair = rankNameGoalPair.Keys.ElementAt(currentGoalIndex);
                    OnNewMamontTitleReached?.Invoke(pair.Name, pair.NamePicture);
                    
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

        private void UpdateMamontTitle(string titleName, Sprite _)
        {
            UpdateMamontTitleAsync(titleName).Forget();
        }
        private async UniTask UpdateMamontTitleAsync(string titleName)
        {
            mamontTitle.text = titleName;

            var mamontObject = mamontTitle.gameObject;
            await mamontObject.transform.DOScale(mamontTitleScale, 0.2f).SetLoops(2, LoopType.Yoyo);
            mamontObject.transform.localScale = new Vector3(1,1,1);
        }
        private void AddProgress(long amount)
        {
            currentValue += amount;
        
            currentValue = Mathf.Clamp(currentValue, 0, maxValue);
        }
    }
}
