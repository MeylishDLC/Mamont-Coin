using System;
using Script.Managers;
using Script.Sound;
using Zenject;

namespace Script.Core.Popups
{
    public class PaidAdPopup: Popup
    {
        public static event Action OnPaidPopupClick;
        public override void CloseApp()
        {
            OnPaidPopupClick?.Invoke();
            base.CloseApp();
        }
    }
}