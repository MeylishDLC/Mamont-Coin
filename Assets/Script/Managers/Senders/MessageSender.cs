using Script.Data.Dialogues;
using UnityEngine;

namespace Script.Managers.Senders
{
    public abstract class MessageSender: MonoBehaviour
    {
        [SerializeField] public GameObject TextObject;
        [SerializeField] public SerializedDictionary<string, DialogueSpeakerPair> Dialogues;
        [SerializeField] public int DelayBetweenMessagesMillisecond;
        public abstract void StartDialogueSequence(string dialogueKey);
    }
}