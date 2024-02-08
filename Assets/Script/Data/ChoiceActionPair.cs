using UnityEngine;
using UnityEngine.Events;

namespace Script.Data
{
    [System.Serializable]
    public class ChoiceActionPair
    {
        [field:SerializeField] public string ChoiceName { get; private set; }
        [field:SerializeField] public UnityEvent Event { get; private set; }
    }
}