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
            Container.Bind<FMODEvents>().FromInstance(FMODEvents).AsSingle();
        }
    }
}