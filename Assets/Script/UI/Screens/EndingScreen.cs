using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class EndingScreen : MonoBehaviour
{
    //todo: ????? some ending better than this shit
    private TypeText text;
    private void Start()
    {
        text = gameObject.GetComponentInChildren<TypeText>();
        text.enabled = false;
        
        OnAppearAsync().Forget();
    }

    private async UniTask OnAppearAsync()
    {
        await gameObject.transform.DOScale(1, 0.1f).ToUniTask();
        text.enabled = true;
    } 
}
