using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressHandler : MonoBehaviour
{
    [Header("Ranks")] 
    [SerializeField] private Rank[] ranks;
    
    [Header("Mamont Title")] 
    [SerializeField] private GameObject mamontTitleObject;
    [SerializeField] private float mamontTitleScale;
    private TextMeshProUGUI mamontTitleText;
    
    [Header("Progress Bar")]
    [SerializeField] private Slider progressBar;
    private float currentValue;
    private float maxValue;
    private int currentGoalIndex;

    private void Start()
    {
        currentValue = GameManager.Clicks;
        currentGoalIndex = 1;
        maxValue = ranks[currentGoalIndex].RankGoal;

        mamontTitleText = mamontTitleObject.GetComponent<TextMeshProUGUI>();
        mamontTitleText.text = ranks[0].RankName;
        
        
        Events.ClicksUpdated += UpdateProgress;
    }

    private void UpdateProgress()
    {
        progressBar.value = currentValue / maxValue;
        if (currentValue == maxValue)
        {
            if (currentGoalIndex < ranks.Length)
            {
                UpdateMamontTitle(ranks[currentGoalIndex].RankName).Forget();
                currentGoalIndex++;
                maxValue = ranks[currentGoalIndex].RankGoal;
            }
            else
            {
                Debug.Log("All goals are finished");
            }
        }
    }
    
    private async UniTask UpdateMamontTitle(string titleName)
    {
        mamontTitleText.text = titleName;
        await mamontTitleObject.transform.DOScale(mamontTitleScale, 0.2f).SetLoops(2, LoopType.Yoyo);
    }

    public void AddProgress(int amount)
    {
        currentValue += amount;
        
        currentValue = Mathf.Clamp(currentValue, 0, maxValue);
    }
}
