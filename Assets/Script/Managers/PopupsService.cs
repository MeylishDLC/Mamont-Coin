using System;
using System.Collections.Generic;
using Script.Core.Popups;
using Script.Sound;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


namespace Script.Managers
{
    public class PopupsService: MonoBehaviour
    {
        [SerializeField] private GameObject spawnParent;
        [SerializeField] private List<Popup> popupsToManage;

        private Vector2 GetRandomPosition()
        {
            var randomX = Random.Range(0f, Screen.width);
            var randomY = Random.Range(0f, Screen.height);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(spawnParent.GetComponent<RectTransform>(), new Vector2(randomX, randomY), Camera.main, out var randomPositionLocal);

            return randomPositionLocal;
        }
        public GameObject SpawnRandomly(params GameObject[] popups)
        {
            var randomPopup = GetRandomPopupPrefab(popups);
            var randomPos = GetRandomPosition();
            
            var popupWindow = Instantiate(randomPopup, spawnParent.transform);
            popupWindow.transform.localPosition = randomPos;
            return popupWindow;
        }
        
        public GameObject GetRandomPopupPrefab(params GameObject[] objectsList)
        {
            var randomWindow = objectsList[Random.Range(0, objectsList.Length)];
            return randomWindow;
        }
        private void ClearParent()
        {
            while (spawnParent.transform.childCount > 0) {
                DestroyImmediate(spawnParent.transform.GetChild(0).gameObject);
            }
        }
        public void DisableAllPopups(bool clearParent = false)
        {
            foreach (var popup in popupsToManage)
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
