using System.ComponentModel;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Script.Core.Popups.Spawns
{
    [CreateAssetMenu(fileName = "CallSpawner", menuName = "Spawners/CallSpawner")]
    public class CallSpawner: ScriptableObject, ISpawner
    {
        public bool SpawnActive { get; set; }
        [field:SerializeField] public int FrequencyMilliseconds { get; set; }
        [SerializeField] private int callsAmount;
        [SerializeField] private Popup popup; 
        [SerializeField] private Vector3 initSpawnPosition;

        private PopupContainer spawnParent;
        private CancellationTokenSource bounceCts;
        
        public void StartSpawn()
        {
            GetSpawnParent();
            
            GameManager.OnGameEnd += ClearSpawnParent;
            GameManager.OnGameEnd += StopSpawn;
            PopupAppearAsync().Forget();
        }
        private void GetSpawnParent()
        {
            spawnParent = FindAnyObjectByType<PopupContainer>();
            if (spawnParent is null)
            {
                Debug.LogError("No popup container in scene was found.");
            }
        }
        private async UniTask PopupAppearAsync()
        {
            SpawnActive = true;
            for (int i = 0; i < callsAmount; i++)
            {
                var popupWindow = InstantiateAndInject(popup).GetComponent<Popup>();
                popupWindow.transform.localPosition = initSpawnPosition;
                
                popupWindow.OpenApp();
                
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();

                bounceCts = new CancellationTokenSource();
                
                Bounce(popupWindow, bounceCts.Token).Forget();
                
                await UniTask.Delay(FrequencyMilliseconds);
       
                popupWindow.CloseApp();
                bounceCts.Cancel();
                
                await UniTask.Delay(300);
            }
            SpawnActive = false;
        }
        private GameObject InstantiateAndInject(Popup popup)
        {
            var projectContext = FindFirstObjectByType<ProjectContext>();
            
            var obj = projectContext.Container.InstantiatePrefab
                (popup, popup.transform.localPosition, Quaternion.identity, spawnParent.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            return obj;
        }

        private async UniTask Bounce(Popup popupWindow, CancellationToken token)
        {
            while (!token.IsCancellationRequested && popupWindow.isActiveAndEnabled)
            {
                await popupWindow.transform.DOScale(0.95f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask(cancellationToken: token);
                await UniTask.Delay(100, cancellationToken: token);
            }
            bounceCts.Dispose();
        }
        public void StopSpawn()
        {
            GameManager.OnGameEnd -= ClearSpawnParent;
            GameManager.OnGameEnd -= StopSpawn;
        }

        public void ClearSpawnParent()
        {
            if (spawnParent is null)
            {
                return;
            }
            spawnParent.Clear();
        }
    }
}