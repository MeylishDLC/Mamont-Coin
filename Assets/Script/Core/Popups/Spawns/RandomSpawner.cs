using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NUnit.Framework;
using Script.Managers;
using UnityEngine;
using Zenject;

namespace Script.Core.Popups.Spawns
{
    [CreateAssetMenu(fileName = "RandomSpawner", menuName = "Spawners/RandomSpawner")]
    public class RandomSpawner: ScriptableObject, ISpawner
    {
        public bool SpawnActive { get; set; }
        [field:SerializeField] public int FrequencyMilliseconds { get; set; }
        [SerializeField] private List<Popup> popups;
        private PopupContainer spawnParent;
        
        public void StartSpawn()
        {
            spawnParent = FindAnyObjectByType<PopupContainer>();
            if (spawnParent is null)
            {
                Debug.LogError("No popup container in scene was found.");
                return;
            }
            
            GameManager.OnGameEnd += ClearSpawnParent;
            StartSpawnAsync().Forget();
        }

        public void StopSpawn()
        {
            GameManager.OnGameEnd -= ClearSpawnParent;
            SpawnActive = false;
        }

        public void ClearSpawnParent()
        {
            if (spawnParent is null)
            {
                return;
            }
            spawnParent.Clear();
        }
        public Vector2 GetRandomPosition()
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), Camera.main, out var randomPositionLocal);

            return randomPositionLocal;
        } 
        public Vector2 GetRandomPosition(PopupContainer popupContainer)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(popupContainer.GetComponent<RectTransform>(), new Vector2(randomX, randomY), Camera.main, out var randomPositionLocal);

            return randomPositionLocal;
        }
        private Popup GetRandomPopup()
        {
            var randomWindow = popups[Random.Range(0, popups.Count)];
            return randomWindow;
        }

        private Popup SpawnObject()
        {
            var randomPopup = GetRandomPopup();
            var randomPos = GetRandomPosition();

            var popupWindow = InstantiateAndInject(randomPopup, randomPos);
            popupWindow.transform.localScale = new Vector3(1, 1, 1);
            popupWindow.transform.localPosition = randomPos;
            
            var popup = popupWindow.GetComponent<Popup>();
            return popup;
        }

        private GameObject InstantiateAndInject(Popup popup, Vector3 position)
        {
            var projectContext = FindFirstObjectByType<ProjectContext>();
            
            var obj = projectContext.Container.InstantiatePrefab
            (popup, position, Quaternion.identity, spawnParent.transform);
            return obj;
        }
        private async UniTask StartSpawnAsync()
        {
            SpawnActive = true;
            while (SpawnActive)
            {
                var obj = SpawnObject();
                obj.OpenApp();
                await UniTask.Delay(FrequencyMilliseconds);
            }
        }
        
    }
}