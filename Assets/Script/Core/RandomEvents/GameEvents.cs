using Script.Managers.Senders;
using UnityEngine;
using Zenject;

namespace Script.Core.RandomEvents
{
    public class GameEvents: MonoBehaviour
    {
        [SerializeField] private ArtClubEvent artEvent;
        [SerializeField] private ChineseTutorEvent chineseEvent;
        [SerializeField] private AnapaEvent anapaEvent;
        
        private AskaMessageSender askaMessageSender;
        private SkampMessageSender skampMessageSender;
        
        [Inject]
        public void Construct(SkampMessageSender skampMessageSender, AskaMessageSender askaMessageSender)
        {
            this.skampMessageSender = skampMessageSender;
            this.askaMessageSender = askaMessageSender;
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

        public void StartAnapaEvent()
        {
            anapaEvent.Construct(askaMessageSender);
            anapaEvent.StartEvent();
        }
    }
}