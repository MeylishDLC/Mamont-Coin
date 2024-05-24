﻿using Script.Core;
using Script.Core.Popups;
using Script.Data;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public class MainSceneInstaller: MonoInstaller
    {
        public ChatManager ChatManager;
        public override void InstallBindings()
        {
            BindDataBank();
            BindBoostsService();
            BindClickHandler();
            BindChatManager();
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
        private void BindChatManager()
        {
            Container.Bind<ChatManager>().FromInstance(ChatManager).AsSingle();
        }
        
    }
}