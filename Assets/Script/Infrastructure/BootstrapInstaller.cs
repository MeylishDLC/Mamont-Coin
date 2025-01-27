﻿using FMODUnity;
using Script.Managers;
using Script.Sound;
using Zenject;

namespace Script.Infrastructure
{
    public class BootstrapInstaller: MonoInstaller
    {
        public AudioManager AudioManagerPrefab;
        public FMODEvents FMODEvents;
        public override void InstallBindings()
        {
            BindAudioManager();
            BindFMODEvents();
        }

        private void BindAudioManager()
        {
            AudioManager audioManager = Container.InstantiatePrefabForComponent<AudioManager>(AudioManagerPrefab);
            Container.Bind<AudioManager>().FromInstance(audioManager).AsSingle();
        }
        private void BindFMODEvents()
        {
            var audioManager = Container.Resolve<AudioManager>();
            Container.Bind<FMODEvents>().FromInstance(FMODEvents).AsSingle();
            
            //attempt to fix the trouble with getting busses in WebGL
            audioManager.MasterBus = RuntimeManager.GetBus("bus:/");
            audioManager.MusicBus = RuntimeManager.GetBus("bus:/Music");
            audioManager.SFXBus = RuntimeManager.GetBus("bus:/SFX");
        }
    }
}