using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Script.Managers;
using Script.Sound;
using UnityEngine;
using Zenject;

namespace Script.Core.Popups
{
    public class TrojanWarningPopups: Popup
    {
        public override void OpenApp()
        {
            AudioManager.PlayOneShot(FMODEvents.errorSound);
            base.OpenApp();
        }
    }
}