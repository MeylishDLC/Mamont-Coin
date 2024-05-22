using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class DragnDrop : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform draggableObjectRectTransform;

        private void OnValidate()
        {
            if (draggableObjectRectTransform != null)
            {
                return;
            }

            if (GetComponent<RectTransform>())
            {
                draggableObjectRectTransform = GetComponent<RectTransform>();
            }
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            draggableObjectRectTransform.anchoredPosition += eventData.delta;
        }
    
    }
}