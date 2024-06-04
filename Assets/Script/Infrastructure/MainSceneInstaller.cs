using Script.Core;
using Script.Data;
using Script.Managers;
using Script.Managers.Senders;
using Zenject;

namespace Script.Infrastructure
{
    public class MainSceneInstaller: MonoInstaller
    {
        public SkampMessageSender skampMessageSender;
        public AskaMessageSender askaMessageSender;
        public override void InstallBindings()
        {
            BindDataBank();
            BindBoostsService();
            BindClickHandler();
            BindSenders();
        }
        private void BindDataBank()
        {
            Container.Bind<IDataBank>().To<DataBank>().AsSingle();
        }
        private void BindClickHandler()
        {
            var dataBank = Container.Resolve<IDataBank>();
            Container.Bind<ClickHandler>().AsSingle().WithArguments(dataBank);
        }
        private void BindBoostsService()
        {
            Container.Bind<SpecificBoostSetter>().AsSingle();
        }
        private void BindSenders()
        {
            Container.Bind<SkampMessageSender>().FromInstance(skampMessageSender).AsSingle();
            Container.Bind<AskaMessageSender>().FromInstance(askaMessageSender).AsSingle();
        }
        
    }
}