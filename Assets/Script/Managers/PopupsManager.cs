using System;
using System.Collections.Generic;
using Script.Core.Popups;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Script.Managers
{
    public class PopupsManager: MonoBehaviour
    {
        [SerializeField] private GameObject spawnParent;
        [SerializeField] private List<Popup> popups;

        private List<GameObject> popupContainers;

        #region SetInstance
        public static PopupsManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
        }
        #endregion

        
        public GameObject RandomPositionSpawn(List<GameObject> objectsList)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), Camera.main, out Vector2 randomPositionLocal);
            
            return SpawnRandomObject(randomPositionLocal, objectsList);
        }

        public GameObject SpawnRandomObject(Vector2 position, List<GameObject> objectsList)
        {
            var randomWindow = objectsList[Random.Range(0, objectsList.Count)];
            var popupWindow = Instantiate(randomWindow, spawnParent.transform);
            popupWindow.transform.localPosition = position;
            return popupWindow;
        }
        private void ClearParent()
        {
            while (spawnParent.transform.childCount > 0) {
                DestroyImmediate(spawnParent.transform.GetChild(0).gameObject);
            }
        }
        public void DisableAllPopups(bool clearParent = false)
        {
            foreach (var popup in popups)
            {
                popup.isActive = false;
            }
            if (clearParent)
            {
                ClearParent();
            }
        }
        
    }
}
