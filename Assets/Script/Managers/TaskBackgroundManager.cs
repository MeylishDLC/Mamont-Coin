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
    [Header("Autoclicker")] 
    [SerializeField] private int clickFrequencyMilliseconds;
    public bool AutoClickEnabled;
    
    //todo: make it work
    /*private bool autoClickEnabled;
    public bool AutoClickEnabled
    {
        get
        {
            return autoClickEnabled;
        }
        set
        {
            autoClickEnabled = value;
            if (value)
            {
                AutoClickAsync().Forget();
            }
        }
    }*/

    [Header("Double Click Chance")] 
    [SerializeField] private int percentageChanceOfDoubleClick;
    public bool doubleClickChanceEnabled;

    [Header("Auto Pop-up Windows")] 
    [SerializeField] private int appearFrequencyMilliseconds;
    [SerializeField] private List<GameObject> popupWindows;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject spawnParent;
    public bool AutoPopupWindowsEnabled;

    private void Start()
    {
        Events.AutoClickEnabled += AutoClick;
        Events.AutoWindowsAppearEnabled += PopupWindowAppear;
        Events.ClicksUpdated += DoubleClickChance;
        
        if (AutoPopupWindowsEnabled)
        {
            PopupWindowAppearAsync().Forget();
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
        while (AutoPopupWindowsEnabled)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);
            var randomWindow = popupWindows[Random.Range(0, popupWindows.Count - 1)];
            
            var randomPositionWorld = mainCamera.ScreenToWorldPoint(new Vector3(randomX, randomY, mainCamera.nearClipPlane));
            
            var popupWindow = Instantiate(randomWindow, randomPositionWorld, Quaternion.identity, spawnParent.transform);
            
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
        while (true)
        {
            await UniTask.Delay(clickFrequencyMilliseconds);
            GameManager.Clicks++;
            Events.ClicksUpdated?.Invoke();
        }
    }
    private void AutoClick()
    {
        AutoClickAsync().Forget();
    }
    
    
}
