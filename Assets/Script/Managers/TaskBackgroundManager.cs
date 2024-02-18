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
    public static int autoClickAmount = 1;

    [Header("Double Click Chance")] 
    [SerializeField] private int percentageChanceOfDoubleClick;
    public static int doubleClickAmount = 2;

    [Header("Auto Pop-up Trojan Warnings")]
    [SerializeField] private int warningAppearFrequencyMilliseconds;
    [SerializeField] private List<GameObject> popupWarnings;
    public bool trojanWarningsActive;
    
    [Header("Auto Pop-up Paid Windows")] 
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

    private void Start()
    {
        if (trojanWarningsActive)
        {
            TrojanWarningAppear();
        }
    }

    public static TaskBackgroundManager GetInstance()
    {
        return instance;
    }

    public int DoubleClickChance()
    {
        var chance = Random.Range(1, 100);
        if (chance <= percentageChanceOfDoubleClick)
        {
            Debug.Log($"Double click = {doubleClickAmount}");
            return doubleClickAmount;
        }
        return 1;
    }

    private GameObject RandomSpawn(List<GameObject> windowsList)
    {
        var randomX = Random.Range(0f, Screen.width);
        var randomY = Random.Range(0f, Screen.height);
        var randomWindow = windowsList[Random.Range(0, windowsList.Count)];

        RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), mainCamera, out Vector2 randomPositionLocal);

        var popupWindow = Instantiate(randomWindow, spawnParent.transform);
        popupWindow.transform.localPosition = randomPositionLocal;
        return popupWindow;
    }
    private async UniTask PopupWindowAppearAsync()
    {
        while (BoostsManager.GetInstance().autoPopupWindowEnabled)
        {
            var popupWindow = RandomSpawn(popupWindows);
            await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            await UniTask.Delay(appearFrequencyMilliseconds);
        }
    }
    public void PopupWindowAppear()
    {
        BoostsManager.GetInstance().autoPopupWindowEnabled = true;
        PopupWindowAppearAsync().Forget();
    }
    private async UniTask TrojanWarningAppearAsync()
    {
        trojanWarningsActive = true;
        while (trojanWarningsActive)
        {
            var popupWindow = RandomSpawn(popupWarnings);
            await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            await UniTask.Delay(warningAppearFrequencyMilliseconds);
        }
    }
    public void TrojanWarningAppear()
    {
        TrojanWarningAppearAsync().Forget();
    }
    private async UniTask AutoClickAsync()
    {
        while (BoostsManager.GetInstance().autoClickerEnabled)
        {
            await UniTask.Delay(clickFrequencyMilliseconds);
            GameManager.Clicks += autoClickAmount;
            Events.ClicksUpdated?.Invoke();
        }
    }
    public void AutoClick()
    {
        AutoClickAsync().Forget();
    }
    
    
}
