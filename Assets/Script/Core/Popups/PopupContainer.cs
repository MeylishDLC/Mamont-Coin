using UnityEngine;

namespace Script.Core.Popups
{
    public class PopupContainer: MonoBehaviour
    {
        public void Clear()
        {
            while (gameObject.transform.childCount > 0) {
                DestroyImmediate(gameObject.transform.GetChild(0).gameObject);
            }
        }
    }
}