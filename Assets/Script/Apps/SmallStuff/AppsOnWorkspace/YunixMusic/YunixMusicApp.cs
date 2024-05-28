using System.Collections.Generic;
using FMOD.Studio;
using Script.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.YunixMusic
{
    public class YunixMusicApp: BasicWorkspaceApp
    {
        public string CurrentMusicName { get; set; } = "Default Music";

        [Header("Music")]
        [SerializeField] private List<Music> musicList;
        [SerializeField] private TMP_Text currentMusic;
        [SerializeField] private TMP_Text currentArtist;
        
        [Header("Pause")]
        [SerializeField] private Button pauseButton;
        [SerializeField] private Sprite pauseSprite;
        [SerializeField] private Sprite playSprite;

        private Image pauseImage;
        private AudioManager audioManager;
        private bool paused;
        
        [Inject]
        public void Construct(AudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        protected override void Start()
        {
            base.Start();
            pauseButton.onClick.AddListener(PauseMusic);
            Music.OnMusicChange += SetMusic;

            pauseImage = pauseButton.GetComponent<Image>();
            pauseImage.sprite = playSprite;
            foreach (var music in musicList)
            {
                music.Button.onClick.AddListener(music.ChangeMusic);
            }
        }

        private void SetMusic(Music music)
        {
            if (CurrentMusicName == music.Name)
            {
                return;
            }
            
            audioManager.StopMusic(CurrentMusicName, STOP_MODE.ALLOWFADEOUT);
            
            audioManager.InitializeMusic(music.Name, music.Sound);
            CurrentMusicName = music.Name;
            
            currentMusic.text = music.Name;
            currentArtist.text = music.Artist;
        }

        private void PauseMusic()
        {
            if (paused)
            {
                paused = false;
                pauseImage.sprite = playSprite;
            }
            else
            {
                paused = true;
                pauseImage.sprite = pauseSprite;
            }
            audioManager.PauseMusic(CurrentMusicName, paused);
        }
    }
}