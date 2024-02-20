using System;
using UnityEngine;
using UnityEngine.UI;

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

        private void Awake()
        {
            volumeSlider = GetComponentInChildren<Slider>();
        }

        private void Update()
        {
            switch (volumeType)
            {
                case VolumeType.MASTER:
                    volumeSlider.value = AudioManager.instance.masterVolume;
                    break;
                case VolumeType.MUSIC:
                    volumeSlider.value = AudioManager.instance.musicVolume;
                    break;
                case VolumeType.SFX:
                    volumeSlider.value = AudioManager.instance.SFXVolume;
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
                    AudioManager.instance.masterVolume = volumeSlider.value;
                    break;
                case VolumeType.MUSIC:
                    AudioManager.instance.musicVolume = volumeSlider.value;
                    break;
                case VolumeType.SFX:
                    AudioManager.instance.SFXVolume = volumeSlider.value;
                    break;
                default:
                    Debug.LogWarning("Volume type isn't valid");
                    break;
            }
        }
    }
}