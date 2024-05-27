using FMODUnity;
using UnityEngine;

namespace Script.Sound
{
    [CreateAssetMenu]
    public class FMODEvents : ScriptableObject
    {
        [field: Header("Music")] 
        [field: SerializeField] public EventReference defaultMusic { get; private set; }

        [field: Header("UI SFX")]
        [field: SerializeField] public EventReference skypeMessageSound { get; private set; }
        [field: SerializeField] public EventReference icqMessageSound { get; private set; }
        [field: SerializeField] public EventReference clickSound { get; private set; }
        [field: SerializeField] public EventReference buySound { get; private set; }
        [field: SerializeField] public EventReference boostChoiceSound { get; private set; }

        [field: Header("Popup Windows SFX")] 
        [field: SerializeField] public EventReference errorSound { get; private set; }
        [field: SerializeField] public EventReference popupADSound { get; private set; }
        
        [field: Header("Windows Specific Sounds")]
        [field: SerializeField] public EventReference windowsGreetingSound { get; private set; }
        
        [field: Header("Duralingo Sounds")]
        [field: SerializeField] public EventReference skypeCallSound { get; private set; }
        [field: SerializeField] public EventReference duralingoCorrect { get; private set; }
        [field: SerializeField] public EventReference duralingoWrong { get; private set; }
        [field: SerializeField] public EventReference duralingoScreamer { get; private set; }
        
 
    }
}