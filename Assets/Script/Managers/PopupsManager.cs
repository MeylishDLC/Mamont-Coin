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

        
        public GameObject RandomSpawn(List<GameObject> windowsList)
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);
            var randomWindow = windowsList[Random.Range(0, windowsList.Count)];

            RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), Camera.main, out Vector2 randomPositionLocal);

            var popupWindow = Instantiate(randomWindow, spawnParent.transform);
            popupWindow.transform.localPosition = randomPositionLocal;
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
