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
        [SerializeField] private Vector2 spawnPosition;

        private PopupContainer spawnParent;
        private CancellationTokenSource bounceCts;
        
        public void StartSpawn()
        {
            spawnParent = FindAnyObjectByType<PopupContainer>();
            if (spawnParent is null)
            {
                Debug.LogError("No popup container in scene was found.");
                return;
            }
            
            GameManager.OnGameEnd += ClearSpawnParent;
            PopupAppearAsync().Forget();
        }
        private async UniTask PopupAppearAsync()
        {
            SpawnActive = true;
            for (int i = 0; i < callsAmount; i++)
            {
                var popupWindow = InstantiateAndInject(popup, spawnPosition).GetComponent<Popup>();
                popupWindow.transform.localScale = new Vector3(1, 1, 1);
                popupWindow.transform.localPosition = spawnPosition;

                popupWindow.OpenApp();
                
                await popupWindow.transform.DOScale(0.9f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask();

                bounceCts = new CancellationTokenSource();
                
                Bounce(popupWindow, bounceCts.Token).Forget();
                
                await UniTask.Delay(FrequencyMilliseconds);
       
                bounceCts.Cancel();
                popupWindow.CloseApp();
                
                await UniTask.Delay(300);
            }
            SpawnActive = false;
        }
        private GameObject InstantiateAndInject(Popup popup, Vector3 position)
        {
            var projectContext = FindFirstObjectByType<ProjectContext>();
            
            var obj = projectContext.Container.InstantiatePrefab
                (popup, position, Quaternion.identity, spawnParent.transform);
            return obj;
        }

        private async UniTask Bounce(Popup popupWindow, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                await popupWindow.transform.DOScale(0.95f, 0.2f).SetLoops(2, LoopType.Yoyo).ToUniTask(cancellationToken: token);
                await UniTask.Delay(100, cancellationToken: token);
            }
            bounceCts.Dispose();
        }
        public void StopSpawn()
        {
            GameManager.OnGameEnd -= ClearSpawnParent;
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