using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Data
{
    [System.Serializable]
    public class DialogueSpeakerPair
    { 
        [field: SerializeField] public Character character;
        [TextArea]
        [field: SerializeField] public List<string> dialogueLines;
    }
}