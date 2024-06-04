using Script.Managers;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Sound
{
    public class VolumeSlider : MonoBehaviour
    {
        private enum VolumeType
        {
            MASTER,
            MUSIC,
            SFX
        }

        [Header("Type")] 
        [SerializeField] private VolumeType volumeType;
        private Slider volumeSlider;
        private AudioManager audioManager;
        
        [Inject]
        public void Construct(AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }
        private void Awake()
        {
            volumeSlider = GetComponentInChildren<Slider>();
        }

        private void Update()
        {
            switch (volumeType)
            {
                case VolumeType.MASTER:
                    volumeSlider.value = audioManager.MasterVolume;
                    break;
                case VolumeType.MUSIC:
                    volumeSlider.value = audioManager.MusicVolume;
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = audioManager.SFXVolume;
                    break;
                default:
                    Debug.LogWarning("Volume type isn't valid");
                    break;
            }
        }

        public void OnSliderValueChanged()
        {
            switch (volumeType)
            {
                case VolumeType.MASTER:
                    audioManager.MasterVolume = volumeSlider.value;
                    break;
                case VolumeType.MUSIC:
                    audioManager.MusicVolume = volumeSlider.value;
                    break;
                case VolumeType.SFX:
                    audioManager.SFXVolume = volumeSlider.value;
                    break;
                default:
                    Debug.LogWarning("Volume type isn't valid");
                    break;
            }
        }
    }
}