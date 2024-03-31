using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextObject : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private Vector2 originalPosition;
    private GameObject textFieldObject;
    public bool isAdded { get; private set; }
    void Start()
    {
        originalPosition = transform.position;
        textFieldObject = TextField.instance.gameObject;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Do nothing
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
            if (result.gameObject == textFieldObject)
            {
                isOverTargetUIObject = true;
                break;
            }
        }

        if (isOverTargetUIObject)
        {
            TextField.instance.AddWordToSentence(this);
            isAdded = true;
        }
        else
        {
            TextField.instance.RemoveWordFromSentence(this);
            isAdded = false;
            transform.position = originalPosition;
        }
        TextField.instance.RefreshLayout();
    }
}
