using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.UI
{
    public class DragnDrop : MonoBehaviour, IDragHandler
    {
        //todo: remake w new input system
        [SerializeField] private RectTransform draggableObjectRectTransform;
        public virtual void OnDrag(PointerEventData eventData)
        {
            draggableObjectRectTransform.anchoredPosition += eventData.delta;
        }
    
    }
}