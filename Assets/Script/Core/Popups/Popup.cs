using System.Collections.Generic;
using UnityEngine;

namespace Script.Core.Popups
{
    public class Popup: ScriptableObject
    {
        [field:SerializeField]public int AppearIntervalMilliseconds { get; private set; }
        [field:SerializeField]public List<GameObject> Popups { get; private set; }
        public bool isActive { get; set; }
        public virtual void PopupAppear(){}
    }
}