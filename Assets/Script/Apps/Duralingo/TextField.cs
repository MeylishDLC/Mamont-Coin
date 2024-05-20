using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.Duralingo
{
    public class TextField : MonoBehaviour
    {
        [SerializeField] private List<TextObject> sentence;
        [SerializeField] private GameObject textObjectsContainer;
        private readonly List<TextObject> currentSentence = new();
        private GridLayoutGroup layoutGroup;

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
            }
        }

        public void RemoveWordFromSentence(TextObject textObject)
        {
            if (textObject.isAdded)
            {
                textObject.gameObject.transform.SetParent(textObjectsContainer.transform);
                currentSentence.Remove(textObject);
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
}
