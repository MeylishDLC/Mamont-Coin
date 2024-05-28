using System;
using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.YunixMusic
{
    [System.Serializable]
    public class Music
    {
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public Image BackgroundImage { get; private set; }
        [field: SerializeField] public EventReference Sound { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Artist { get; private set; }

        public static event Action<Music> OnMusicChange;

        public void ChangeMusic() => OnMusicChange?.Invoke(this);
    }
}