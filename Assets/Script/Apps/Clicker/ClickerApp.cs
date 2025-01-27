using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core;
using Script.Data;
using Script.Managers;
using Script.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;

namespace Script.Apps.Clicker
{
    public class ClickerApp : MonoBehaviour
    {
        [Header("Clicker")] 
        [SerializeField] private GameObject errorMessagePrefab;
        [SerializeField] private GameObject spawnParent;
        [SerializeField] private float errorWindowAnimationScale;
        [SerializeField] private Button clickerCloseButton;
    
        [Header("Clicker Button")]
        [SerializeField] private Button clickerButton;
        [SerializeField] private float minScale;
        [SerializeField] private float scaleDuration;
        [SerializeField] private ParticleSystem[] particlePrefabs;
        
        [Header("Values View")]
        [SerializeField] private TMP_Text counterText;
        [SerializeField] private TMP_Text multiplierText;
        [SerializeField] private TMP_Text coinToRubleText;
        [SerializeField] private int coinExchangeRate;
        
        private bool isAnimated;
        private ClickHandler clickHandler;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        private IDataBank dataBank;

        [Inject]
        public void Construct(IDataBank dataBank, ClickHandler clickHandler, AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.dataBank = dataBank;
            this.clickHandler = clickHandler;

            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }

        private void Start()
        {
            counterText.text = dataBank.Clicks.ToString();
            multiplierText.text = dataBank.Multiplier + " кликов";
            coinToRubleText.text = "0 руб.";
            
            clickerCloseButton.onClick.AddListener(OnClickerCloseButtonPress);
            
            clickerButton.onClick.AddListener(() => clickHandler.Increment(dataBank.Multiplier));
            clickerButton.onClick.AddListener(ParticleSpawn);
            clickerButton.onClick.AddListener(AnimateClicker);

            ClickHandler.ClicksUpdated += OnClicksUpdated;
        }
        private void OnDestroy()
        {
            clickerButton.onClick.RemoveAllListeners();
            clickerCloseButton.onClick.RemoveAllListeners();
            
            ClickHandler.ClicksUpdated -= OnClicksUpdated;
        }

        public void OnClickerCloseButtonPress()
        {
            var errorWindow = Instantiate(errorMessagePrefab, spawnParent.transform);
            
            audioManager.PlayOneShot(FMODEvents.errorSound);
        
            var rectTransform = errorWindow.GetComponent<RectTransform>();
            var position = rectTransform.anchoredPosition;
        
            position.x += Random.Range(-100, 100);
            position.y += Random.Range(-50, 50);
        
            rectTransform.anchoredPosition = position;
            errorWindow.transform.DOScale(errorWindowAnimationScale, 0.1f).SetLoops(2, LoopType.Yoyo);
        }
        public void ParticleSpawn()
        {
            foreach (var particle in particlePrefabs)
            {
                Instantiate(particle, clickerButton.GetComponent<RectTransform>().position,
                    Quaternion.identity);
            }
        }
        private void OnClicksUpdated(BigInteger addAmount)
        {
            counterText.text = dataBank.Clicks.ToString();
            multiplierText.text = dataBank.Multiplier + " кликов";
            
            coinToRubleText.text = ConvertToRuble(dataBank.Clicks) + " руб.";
        }
        private double ConvertToRuble(BigInteger clicks)
        {
            var rubles = (double)(decimal)clicks / Convert.ToDouble(coinExchangeRate);
            return rubles;
        }

        private void AnimateClicker()
        {
            if (isAnimated)
                return;
        
            AnimateClickerAsync().Forget();
        }
        private async UniTask AnimateClickerAsync()
        {
            isAnimated = true;
            await clickerButton.gameObject.transform.DOScale(minScale, scaleDuration).SetLoops(2, LoopType.Yoyo);
            isAnimated = false;
        }
    
    }
}

