using Script.Core.Boosts;
using Script.Core.Popups;
using UnityEditor;
using UnityEngine;

namespace Script.CustomEditor
{
    public class PopupsEditor: Editor
    {
        [MenuItem("Assets/Create/Popups", false, 2000)]
        public static void CreatePopup()
        {
            var asset = ScriptableObject.CreateInstance<Popup>();
            ProjectWindowUtil.CreateAsset(asset, "MyScriptableObject.asset");
        }
        [MenuItem("Assets/Create/Popups", true)]
        public static bool ValidateCreateMyCustomObject()
        {
            return true;
        }
    }
}