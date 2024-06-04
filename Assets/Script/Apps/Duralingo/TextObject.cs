using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Apps.Duralingo
{
    public class TextObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        private Vector2 originalPosition;
        [SerializeField] private TextField textField;
        public bool IsAdded { get; private set; }
        void Start()
        {
            originalPosition = transform.localPosition;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            //Do nothing
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            var isOverTargetUIObject = false;
        
            foreach (var result in results)
            {
                if (result.gameObject == textField.gameObject)
                {
                    isOverTargetUIObject = true;
                    break;
                }
            }

            if (isOverTargetUIObject)
            {
                textField.AddWordToSentence(this);
                IsAdded = true;
            }
            else
            {
                textField.RemoveWordFromSentence(this);
                IsAdded = false;
                transform.localPosition = originalPosition;
            }
            textField.RefreshLayout();
        }
    }
}
