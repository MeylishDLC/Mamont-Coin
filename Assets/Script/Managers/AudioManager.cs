using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using Script.Sound;
using UnityEngine;
using Zenject;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace Script.Managers
{
    public class AudioManager : MonoBehaviour
    {
        [BankRef]
        public List<string> banks;
        
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

        private Dictionary<string, EventInstance> eventInstances;

        private EventInstance musicEventInstance;

        private void Awake()
        {
            LoadBanks();
            eventInstances = new();
        }

        private void Update()
        {
            masterBus = RuntimeManager.GetBus("bus:/");
            musicBus = RuntimeManager.GetBus("bus:/Music");
            SFXBus = RuntimeManager.GetBus("bus:/SFX");
            
            masterBus.setVolume(masterVolume);
            musicBus.setVolume(musicVolume);
            SFXBus.setVolume(SFXVolume);
        }

        public void InitializeMusic(string musicName, EventReference musicEventReference)
        {
            musicEventInstance = CreateInstance(musicName,musicEventReference);
            musicEventInstance.start();
        }

        public bool HasMusic(string musicName)
        {
            return eventInstances.ContainsKey(musicName);
        }
        public void StopMusic(string musicName, STOP_MODE stopMode)
        {
            if (!eventInstances.ContainsKey(musicName))
            {
                Debug.LogError($"No music with name {musicName} was found");
            }

            var instance = eventInstances[musicName];
            instance.stop(stopMode);
            instance.release();
            eventInstances.Remove(musicName);
        }

        public void PauseMusic(string musicName, bool paused)
        {
            eventInstances[musicName].setPaused(paused);
        }
        public void SetMusicAct(MusicAct act)
        {
            musicEventInstance.setParameterByName("Act", (float) act);
        }

        public EventInstance CreateInstance(string instanceName, EventReference eventReference)
        {
            var eventInstance = RuntimeManager.CreateInstance(eventReference);
            eventInstances.Add(instanceName, eventInstance);
            return eventInstance;
        }

        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }
        
        private void CleanUp()
        {
            if (eventInstances is null)
            {
                return;
            }
            foreach (EventInstance eventInstance in eventInstances.Values)
            {
                eventInstance.stop(STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        private void LoadBanks()
        {
            foreach (var b in banks)
            {
                RuntimeManager.LoadBank(b, true);
                Debug.Log("Loaded bank " + b);
            }

            RuntimeManager.CoreSystem.mixerSuspend();
            RuntimeManager.CoreSystem.mixerResume();
            
            CheckBanksLoaded().Forget();
        }

        private async UniTask CheckBanksLoaded()
        {
            while (!RuntimeManager.HaveAllBanksLoaded)
            {
                await UniTask.Yield();
            }
        }
    }
}