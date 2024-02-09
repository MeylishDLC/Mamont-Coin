using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class CardForm : MonoBehaviour
{
    public bool numberSet { get; set; }
    public bool nameSet { get; set; }
    public bool daySet { get; set; }
    public bool yearSet { get; set; }
    public bool CVVSet { get; set; }

    [SerializeField] private GameObject endScreen;
    private TypeText text;

    private void Start()
    {
        endScreen.SetActive(false);
        text = endScreen.GetComponentInChildren<TypeText>();
        text.enabled = false;
    }

    private void Update()
    {
        if (numberSet && nameSet && daySet && yearSet && CVVSet)
        {
            EndScreenAppearAsync().Forget();
        }
    }

    private async UniTask EndScreenAppearAsync()
    {
        endScreen.SetActive(true);
        await endScreen.transform.DOScale(1, 0.1f).ToUniTask();
        text.enabled = true;
    }
}
