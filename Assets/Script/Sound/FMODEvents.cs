using System;
using FMODUnity;
using UnityEngine;

namespace Script.Sound
{
    public class FMODEvents : MonoBehaviour
    {
        [field: Header("Music")] 
        [field: SerializeField] public EventReference defaultMusic { get; private set; }

        [field: Header("UI SFX")]
        [field: SerializeField] public EventReference skypeMessageSound { get; private set; }
        [field: SerializeField] public EventReference skypeCallSound { get; private set; }
        [field: SerializeField] public EventReference clickSound { get; private set; }

        [field: Header("Popup Windows SFX")] 
        [field: SerializeField] public EventReference errorSound { get; private set; }
        [field: SerializeField] public EventReference popupADSound { get; private set; }
        
        [field: Header("Windows Specific Sounds")]
        [field: SerializeField] public EventReference windowsGreetingSound { get; private set; }
        public static FMODEvents instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than FMOD Events found");
            }
            instance = this;
        }
    }
}