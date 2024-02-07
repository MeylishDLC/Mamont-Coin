using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TaskBackgroundManager : MonoBehaviour
{
    [Header("AutoClicker")] 
    [SerializeField] private int clickFrequencyMilliseconds;

    [Header("Double Click Chance")] 
    [SerializeField] private int percentageChanceOfDoubleClick;
    
    [Header("Auto Pop-up Windows")] 
    [SerializeField] private int appearFrequencyMilliseconds;
    [SerializeField] private List<GameObject> popupWindows;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject spawnParent;

    private static TaskBackgroundManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one TaskBackgroundManager in the scene.");
        }
        instance = this;
    }

    public static TaskBackgroundManager GetInstance()
    {
        return instance;
    }
    
    private void Start()
    {
        Events.AutoClickEnabled += AutoClick;
        Events.AutoWindowsAppearEnabled += PopupWindowAppear;
        Events.ClicksUpdated += DoubleClickChance;
        
        
        //debug//
        if (GameManager.GetInstance().autoPopupWindowsEnabled)
        {
            Events.AutoWindowsAppearEnabled?.Invoke();
        }
        if (GameManager.GetInstance().doubleClickChanceEnabled)
        {
            Events.DoubleClickChanceEnabled?.Invoke();
        }
        if (GameManager.GetInstance().autoClickEnabled)
        {
            Events.AutoClickEnabled?.Invoke();
        }
    }

    private void DoubleClickChance()
    {
        var chance = Random.Range(1, 100);
        if (chance <= percentageChanceOfDoubleClick)
        {
            GameManager.Clicks++;
            Debug.Log("Double click");
        }
    }

    private async UniTask PopupWindowAppearAsync()
    {
        while (GameManager.GetInstance().autoPopupWindowsEnabled)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);
            var randomWindow = popupWindows[Random.Range(0, popupWindows.Count - 1)];
            
            var randomPositionWorld = mainCamera.ScreenToWorldPoint(new Vector2(randomX, randomY));
            
            var popupWindow = Instantiate(randomWindow, spawnParent.transform);
            popupWindow.transform.localPosition = randomPositionWorld;
            
            await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            await UniTask.Delay(appearFrequencyMilliseconds);
        }
    }
    private void PopupWindowAppear()
    {
        PopupWindowAppearAsync().Forget();
    }
    private async UniTask AutoClickAsync()
    {
        await UniTask.Delay(clickFrequencyMilliseconds);
        GameManager.Clicks++;
        Events.ClicksUpdated?.Invoke();
    }
    private void AutoClick()
    {
        AutoClickAsync().Forget();
    }
    
    
}
