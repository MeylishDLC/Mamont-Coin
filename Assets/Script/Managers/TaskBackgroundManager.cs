using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class TaskBackgroundManager : MonoBehaviour
{
    [Header("Autoclicker")] 
    [SerializeField] private int clickFrequencyMilliseconds;
    private bool autoClickEnabled;
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
    }

    [Header("Percent Chance")] 
    [SerializeField] private float percentageChanceOfDoubleClick;
    public bool doubleClickChanceEnabled;

    //todo: start on event
    private async UniTask AutoClickAsync()
    {
        while (true)
        {
            await UniTask.Delay(clickFrequencyMilliseconds);
            GameManager.Clicks++;
            Events.ClicksUpdated?.Invoke();
        }
    }
    
}
