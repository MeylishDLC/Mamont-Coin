using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.AnapaApp
{
    [Serializable]
    public class Photo
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public Sprite Picture { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
        
        public static event Action<Photo> OnPhotoClicked;

        public void OnButtonClicked() => OnPhotoClicked?.Invoke(this);
    }
}