using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class BackgroundResizer : MonoBehaviour
{
    [SerializeField] private TMP_Text textObject;
    [SerializeField] private RectTransform imageRectTransform;
    [SerializeField] private Vector2 padding;
    bool isOutside;

    private void Update()
    {
        //get rendered values of the text
        textObject.ForceMeshUpdate();
        
        var textInfo = textObject.textInfo;
        var textBounds = textInfo.meshInfo[0].mesh.bounds;

        imageRectTransform.sizeDelta = new Vector2(textBounds.size.x + padding.x, textBounds.size.y + padding.y);
    }
    
}
