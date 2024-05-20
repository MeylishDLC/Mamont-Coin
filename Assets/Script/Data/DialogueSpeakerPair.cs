using System.Collections.Generic;
using UnityEngine;

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