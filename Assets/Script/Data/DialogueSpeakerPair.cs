using System.Collections.Generic;
using UnityEngine;

namespace Script.Data
{
    [System.Serializable]
    public class DialogueSpeakerPair
    {
        [field: SerializeField] public Speaker speaker;
        [TextArea]
        [field: SerializeField] public List<string> dialogueLines;
    }

    public enum Speaker
    {
        Scammer, 
        Hacker
    }
}