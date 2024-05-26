using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Script.Data;
using UnityEngine;

namespace Script.Managers
{
    public abstract class MessageSender: MonoBehaviour
    {
        [SerializeField] public GameObject TextObject;
        [SerializeField] public SerializedDictionary<string, DialogueSpeakerPair> Dialogues;
        [SerializeField] public int DelayBetweenMessagesMillisecond;
        public abstract void StartDialogueSequence(string dialogueKey);
    }
}