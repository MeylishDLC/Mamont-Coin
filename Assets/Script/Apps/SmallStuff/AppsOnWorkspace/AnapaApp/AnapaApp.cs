using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.AnapaApp
{
    public class AnapaApp: BasicWorkspaceApp
    { 
        [SerializeField] private PhotoApp photoApp;
        [SerializeField] private Photo[] photos;

        protected override void Start()
        {
            base.Start();
            foreach (var photo in photos)
            {
                photo.Button.onClick.AddListener(photo.OnButtonClicked);
            }
            Photo.OnPhotoClicked += OpenPhoto;
        }
        private void OpenPhoto(Photo photo)
        {
            if (photoApp.IsOpen)
            {
                photoApp.CloseApp();
            }
            photoApp.Picture.sprite = photo.Picture;
            photoApp.PhotoText.text = photo.Name;
            photoApp.OpenApp();
        }
    }
}