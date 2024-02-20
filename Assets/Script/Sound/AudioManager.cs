using System;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;

namespace Script.Sound
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Volume")] 
        [Range(0,1)]
        public float masterVolume = 1;
        [Range(0,1)]
        public float musicVolume = 1;
        [Range(0,1)]
        public float SFXVolume = 1;

        private Bus masterBus;
        private Bus musicBus;
        private Bus SFXBus;

        private List<EventInstance> eventInstances;
        private List<StudioEventEmitter> eventEmitters;

        private EventInstance musicEventInstance;
        public static AudioManager instance { get; private set; }
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one Audio Manager found");
            }
            instance = this;

            eventEmitters = new List<StudioEventEmitter>();
            eventInstances = new List<EventInstance>();

            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            SFXBus = RuntimeManager.GetBus("bus:/SFX");
        }

        private void Update()
        {
            masterBus.setVolume(masterVolume);
            musicBus.setVolume(musicVolume);
            SFXBus.setVolume(SFXVolume);
        }

        private void Start()
        {
            InitializeMusic(FMODEvents.instance.defaultMusic);
        }

        private void InitializeMusic(EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicEventReference);
            musicEventInstance.start();
        }

        public void SetMusicAct(MusicAct act)
        {
            musicEventInstance.setParameterByName("Act", (float) act);
        }

        public EventInstance CreateInstance(EventReference eventReference)
        {
            var eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(eventInstance);
            return eventInstance;
        }
        
        public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject)
        {
            var emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
            emitter.EventReference = eventReference;
            eventEmitters.Add(emitter);
            return emitter;
        }
        
        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }
    }
}