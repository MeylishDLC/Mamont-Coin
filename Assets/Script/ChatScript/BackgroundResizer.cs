using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(RectTransform))]
public class BackgroundResizer : MonoBehaviour
{
    [SerializeField] private GameObject image;
    [SerializeField] private GameObject text;
    [SerializeField] private float paddingX = 0f;
    [SerializeField] private float paddingY = 0f;

    private RectTransform imageRectTransform;
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        imageRectTransform = image.GetComponent<RectTransform>();
        textMeshPro = text.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textMeshPro.ForceMeshUpdate();
        var preferredHeight = textMeshPro.GetPreferredValues().y;
        imageRectTransform.sizeDelta = new Vector2(imageRectTransform.sizeDelta.x + paddingX, preferredHeight + paddingY);
    }
}
