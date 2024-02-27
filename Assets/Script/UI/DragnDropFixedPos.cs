using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class DragnDropMoveSystem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private RectTransform draggableObjectRectTransform; 
        
        private Vector2 originalPosition;
        private Vector2 originalPositionOffset;

        private void Start()
        {
            originalPosition = draggableObjectRectTransform.anchoredPosition;
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            draggableObjectRectTransform.anchoredPosition += eventData.delta;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            originalPositionOffset = draggableObjectRectTransform.anchoredPosition - originalPosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            draggableObjectRectTransform.anchoredPosition = originalPosition + originalPositionOffset;
        }
    }
}