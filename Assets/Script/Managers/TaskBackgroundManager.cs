using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
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

    [Header("Percent Chance")] 
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
        Events.AutoclickStarted += AutoClick;
        Events.AutoWindowsAppearStarted += PopupWindowAppear;
        
        if (AutoPopupWindowsEnabled)
        {
            PopupWindowAppearAsync().Forget();
        }
    }

    public async UniTask PopupWindowAppearAsync()
    {
        while (true)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);
            var randomWindow = popupWindows[Random.Range(0, popupWindows.Count - 1)];
        
            var randomPositionScreen = new Vector3(randomX, randomY, mainCamera.nearClipPlane);
            var randomPositionWorld = mainCamera.ScreenToWorldPoint(randomPositionScreen);
        
            Instantiate(randomWindow, randomPositionWorld, Quaternion.identity, spawnParent.transform);
        
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
