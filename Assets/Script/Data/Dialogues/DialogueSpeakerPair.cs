using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script.Data.Dialogues
{
    [System.Serializable]
    public class DialogueSpeakerPair
    { 
        [FormerlySerializedAs("chatCharacters")] [field: SerializeField] public ChatCharacter chatCharacter;
        [TextArea]
        [field: SerializeField] public List<string> dialogueLines;
    }
}