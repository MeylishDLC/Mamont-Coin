using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Action = Unity.Android.Gradle.Manifest.Action;

public class TextField : MonoBehaviour
{
    [SerializeField] private List<TextObject> sentence;
    [SerializeField] private GameObject textObjectsContainer;
    private readonly List<TextObject> currentSentence = new();
    private GridLayoutGroup layoutGroup;

    public static TextField instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one TextField in the scene.");
        }
        instance = this;
    }

    private void Start()
    {
        layoutGroup = gameObject.GetComponent<GridLayoutGroup>();
    }

    public void RefreshLayout()
    {
        layoutGroup.SetLayoutHorizontal();
        layoutGroup.SetLayoutVertical();
    }
    public void AddWordToSentence(TextObject textObject)
    {
        if (!textObject.isAdded)
        {
            textObject.gameObject.transform.SetParent(gameObject.transform);
            currentSentence.Add(textObject);
            Debug.Log("Word Added");
        }
    }

    public void RemoveWordFromSentence(TextObject textObject)
    {
        if (textObject.isAdded)
        {
            textObject.gameObject.transform.SetParent(textObjectsContainer.transform);
            currentSentence.Remove(textObject);
            Debug.Log("Word Removed");
        }
    }

    public bool CheckAccuracy()
    {
        if (currentSentence.Count != sentence.Count)
            return false;
        
        for (var i = 0; i < currentSentence.Count; i++)
        {
            if (currentSentence[i] != sentence[i])
            {
                return false;
            }
        }
        return true;
    }
}
