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