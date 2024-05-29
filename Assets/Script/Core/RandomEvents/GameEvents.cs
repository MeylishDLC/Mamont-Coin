using System.ComponentModel;
using Script.Managers;
using Script.Managers.Senders;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Script.Core.RandomEvents
{
    public class GameEvents: MonoBehaviour
    {
        [SerializeField] private ArtClubEvent artEvent;
        [SerializeField] private ChineseTutorEvent chineseEvent;

        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        private AskaMessageSender askaMessageSender;
        private SkampMessageSender skampMessageSender;
        
        [Inject]
        public void Construct(SkampMessageSender skampMessageSender, AskaMessageSender askaMessageSender, 
            AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.skampMessageSender = skampMessageSender;
            this.askaMessageSender = askaMessageSender;
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }

        public void StartChineseEvent()
        {
            chineseEvent.Construct(askaMessageSender);
            chineseEvent.StartEvent();
        }
        public void StartArtEvent()
        {
            artEvent.Construct(askaMessageSender, skampMessageSender);
            artEvent.StartEvent();
        }
    }
}