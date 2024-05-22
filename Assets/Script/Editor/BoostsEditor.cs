using System;
using Script.Core.Boosts;
using UnityEditor;
using UnityEngine;

namespace Script.CustomEditor
{
    public class BoostsEditor : Editor
    {
        [MenuItem("Assets/Create/Boosts", true)]
        public static bool ValidateCreateMyCustomObject()
        {
            return true;
        }
    }
}