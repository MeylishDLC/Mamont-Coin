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
    public int autoclickAmount;

    [Header("Double Click Chance")] 
    [SerializeField] private int percentageChanceOfDoubleClick;

    [Header("Auto Pop-up Windows")] 
    [SerializeField] private int appearFrequencyMilliseconds;
    [SerializeField] private List<GameObject> popupWindows;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject spawnParent;
    public int coinsPerPopupWindow;

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

    public void DoubleClickChance()
    {
        var chance = Random.Range(1, 100);
        if (chance <= percentageChanceOfDoubleClick)
        {
            GameManager.Clicks *= 2;
            Events.ClicksUpdated?.Invoke();
            Debug.Log("Chance click");
        }
    }

    private async UniTask PopupWindowAppearAsync()
    {
        while (BoostsManager.GetInstance().autoPopupWindowEnabled)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);
            var randomWindow = popupWindows[Random.Range(0, popupWindows.Count - 1)];

            RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), mainCamera, out Vector2 randomPositionLocal);

            var popupWindow = Instantiate(randomWindow, spawnParent.transform);
            popupWindow.transform.localPosition = randomPositionLocal;
            
            await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            await UniTask.Delay(appearFrequencyMilliseconds);
        }
    }
    public void PopupWindowAppear()
    {
        BoostsManager.GetInstance().autoPopupWindowEnabled = true;
        PopupWindowAppearAsync().Forget();
    }
    private async UniTask AutoClickAsync(int clicksAmount)
    {
        while (true)
        {
            await UniTask.Delay(clickFrequencyMilliseconds);
            GameManager.Clicks += clicksAmount;
            Events.ClicksUpdated?.Invoke();
            
            if (clicksAmount != autoclickAmount)
                break;
        }
    }
    public void AutoClick(int clicksAmount)
    {
        AutoClickAsync(clicksAmount).Forget();
    }
    
    
}
