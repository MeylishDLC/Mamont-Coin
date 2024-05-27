using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Core.Popups
{
    public class DuralingoScreamer: MonoBehaviour
    {
        [SerializeField] private float horizontalScale;
        [SerializeField] private float scaleDuration;
        [SerializeField] private int delayBeforeDisappear;
        
        private RectTransform screamerRect;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        
        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }

        private void Start()
        {
            screamerRect = GetComponent<RectTransform>();

            DuralingoPopup.OnDuralingoCallClicked += ShowScreamer;
            gameObject.SetActive(false);
        }

        public void ShowScreamer()
        {
            ShowScreamerAsync().Forget();
        }

        private async UniTask ShowScreamerAsync()
        {
            gameObject.SetActive(true);
            audioManager.PlayOneShot(FMODEvents.duralingoScreamer);
            await screamerRect.DOScale(new Vector3(horizontalScale, 1, 0), scaleDuration).ToUniTask();
            await UniTask.Delay(delayBeforeDisappear);
            gameObject.SetActive(false);
            screamerRect.localScale = new Vector3(1, 1, 0);
        }
    }
}