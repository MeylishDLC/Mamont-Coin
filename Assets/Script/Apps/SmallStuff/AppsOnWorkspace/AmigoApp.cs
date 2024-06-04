using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Core.Popups;
using Script.Core.Popups.Spawns;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Script.Apps.SmallStuff.AppsOnWorkspace
{
    public class AmigoApp: BasicWorkspaceApp
    {
        [SerializeField] private GameObject lagScreen;
        [SerializeField] private int delayBeforeLagMilliseconds;
        [SerializeField] private Popup[] adPopupsPrefabs;
        [SerializeField] private int spawnAmount;
        [SerializeField] private RandomSpawner randomSpawner;

        private PopupContainer popupContainer;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        
        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }
        public override void OpenApp()
        {
            if (IsOpen)
            {
                return;
            }
            
            gameObject.transform.SetSiblingIndex(gameObject.transform.parent.childCount - 1);
            
            popupContainer = FindAnyObjectByType<PopupContainer>();
            
            base.OpenApp();
            SpawnPopups().Forget();
        }

        public override void CloseApp()
        {
            CloseAppAsync().Forget();
        }

        private async UniTask CloseAppAsync()
        {
            openIconButton.interactable = false;
            closeButton.interactable = false;
            
            await gameObject.transform.DOScale(scaleOnClose, scaleDuration).ToUniTask();
            gameObject.SetActive(false);
            IsOpen = false;
            gameObject.transform.localPosition = initPos;
            openIconButton.interactable = true;
            closeButton.interactable = true;
            lagScreen.SetActive(false);
        }
        private async UniTask SpawnPopups()
        {
            await UniTask.Delay(delayBeforeLagMilliseconds);
            
            audioManager.PlayOneShot(FMODEvents.appStoppingSound);
            lagScreen.SetActive(true);

            for (int i = 0; i < spawnAmount; i++)
            {
                foreach (var adPopup in adPopupsPrefabs)
                {
                    var pos = randomSpawner.GetRandomPosition(popupContainer);

                    var popup = Instantiate(adPopup, popupContainer.transform);
                    popup.OpenApp();
                    popup.transform.localPosition = pos;
                }
            }
        }
        
    }
}