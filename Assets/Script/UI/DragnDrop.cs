using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragnDrop : MonoBehaviour, IDragHandler
{
    //todo: remake w new input system
    [SerializeField] private RectTransform draggableObjectRectTransform;
    public virtual void OnDrag(PointerEventData eventData)
    {
        draggableObjectRectTransform.anchoredPosition += eventData.delta;
    }
    
}