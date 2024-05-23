using System;

namespace Script.Core.Popups
{
    public class PaidAdPopup: AdPopup
    {
        public static event Action OnPaidPopupClick;
        public override void PopupAppear()
        {
            base.PopupAppear();
            OnPaidPopupClick?.Invoke();
        }
    }
}