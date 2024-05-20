using UnityEngine;

namespace Script.Core.Popups
{
    [CreateAssetMenu]
    public class DuralingoPopup: Popup
    {
        [field: SerializeField] public int DuralingoCallsAmount { get; private set; }
        public override void PopupAppear()
        {
            isActive = true;
            Debug.LogError("spam spam duralingo");
            isActive = false;
        }
    }
}