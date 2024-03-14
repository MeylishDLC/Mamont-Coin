using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Sound;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PopupsManager : MonoBehaviour
{
    public GameObject PopupsContainer;
    
    [Header("Trojan Warnings")]
    [SerializeField] private int warningAppearFrequencyMilliseconds;
    [SerializeField] private List<GameObject> popupWarnings;
    
    [Header("Auto Pop-up AD Windows")] 
    public List<GameObject> popupWindows;
    [SerializeField] private GameObject spawnParent;
    [SerializeField] private int appearFrequencyMilliseconds;
    private bool trojanWarningsActive { get; set; }
    private bool adPopupsActive { get; set; }

    private static PopupsManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one TaskBackgroundManager in the scene.");
        }
        instance = this;
        
        PopupWindowAppearAsync().Forget();
    }
    public static PopupsManager GetInstance()
    {
        return instance;
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
            await UniTask.Delay(appearFrequencyMilliseconds);
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
            await UniTask.Delay(warningAppearFrequencyMilliseconds);
        }
    }
    public void TrojanWarningAppear()
    {
        TrojanWarningAppearAsync().Forget();
    }

    public void DisableAllPopups()
    {
        trojanWarningsActive = false;
        adPopupsActive = false;
    }
    
}
