using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Apps.Duralingo;
using Script.Sound;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PopupsManager : MonoBehaviour
{
    public GameObject PopupsContainer;
    
    [Header("Trojan Warnings")]
    [SerializeField] private int warningAppearIntervalMilliseconds;
    [SerializeField] private List<GameObject> popupWarnings;
    
    [Header("Auto Pop-up AD Windows")] 
    public List<GameObject> popupWindows;
    [SerializeField] private GameObject spawnParent; 
    [SerializeField] private int adAppearIntervalMilliseconds;

    [Header("Duralingo")] 
    [SerializeField]private List<DuralingoGame> duralingoTests;
    [SerializeField] private GameObject duralingoCall;
    [SerializeField] private int duralingoCallIntervalMilliseconds;
    [SerializeField] private int duralingoCallsAmount;
    private int currentDuralingoTestIndex;
    
    private bool trojanWarningsActive { get; set; }
    private bool adPopupsActive { get; set; }

    #region Set Instance

    private static PopupsManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one PopupsManager in the scene.");
        }
        instance = this;
    }
    public static PopupsManager GetInstance()
    {
        return instance;
    }

    #endregion

    private void Start()
    {
        PopupWindowAppearAsync().Forget();

        foreach (var duralingo in duralingoTests)
        {
            duralingo.OnDuralingoLoseGame += DuralingoCallSpamAsync;
        }
    }

    public GameObject RandomSpawn(List<GameObject> windowsList)
    {
        var randomX = Random.Range(0f, Screen.width);
        var randomY = Random.Range(0f, Screen.height);
        var randomWindow = windowsList[Random.Range(0, windowsList.Count)];

        RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), Camera.main, out Vector2 randomPositionLocal);

        var popupWindow = Instantiate(randomWindow, spawnParent.transform);
        popupWindow.transform.localPosition = randomPositionLocal;
        return popupWindow;
    }

    private async UniTask PopupWindowAppearAsync()
    {
        adPopupsActive = true;
        while (adPopupsActive)
        {
            var popupWindow = RandomSpawn(popupWindows);
            popupWindow.GetComponent<PopupWindow>().isPaid = false;
            await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            await UniTask.Delay(adAppearIntervalMilliseconds);
        }
    }
   
    private async UniTask TrojanWarningAppearAsync()
    {
        trojanWarningsActive = true;
        while (trojanWarningsActive)
        {
            var popupWindow = RandomSpawn(popupWarnings);
            AudioManager.instance.PlayOneShot(FMODEvents.instance.errorSound);
            
            await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();
            await UniTask.Delay(warningAppearIntervalMilliseconds);
        }
    }
    public void TrojanWarningAppear()
    {
        TrojanWarningAppearAsync().Forget();
    }

    public void DuralingoAppear()
    {
        var duralingo = duralingoTests[currentDuralingoTestIndex];
        duralingo.OpenApp();
        if (currentDuralingoTestIndex < duralingoTests.Count - 1)
        {
            currentDuralingoTestIndex++;
        }
        else
        {
            Debug.Log("no duralingo tests anymore");
        }
    }
    private void DuralingoCallSpamAsync()
    {
        Debug.Log("Spam Spam");
    }
    public void DisableAllPopups()
    {
        trojanWarningsActive = false;
        adPopupsActive = false;
    }
    
}
