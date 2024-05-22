using System;
using Script.Core.Boosts;
using UnityEditor;
using UnityEngine;

namespace Script.CustomEditor
{
    public class BoostsEditor : Editor
    {
        [MenuItem("Assets/Create/Boosts", false, 2000)]
        public static void CreateBoost()
        {
            var asset = ScriptableObject.CreateInstance<Boost>();
            ProjectWindowUtil.CreateAsset(asset, "MyScriptableObject.asset");
        }
        [MenuItem("Assets/Create/Boosts", true)]
        public static bool ValidateCreateMyCustomObject()
        {
            return true;
        }
    }
}