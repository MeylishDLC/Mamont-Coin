using UnityEngine;

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
                photo.OnPhotoClicked += OpenPhoto;
            }
        }
        
        private void OpenPhoto(Photo photo)
        {
            photoApp.Picture.sprite = photo.Picture;
            photoApp.PhotoText.text = photo.Name;

            if (!photoApp.IsOpen)
            {
                photoApp.OpenApp();
            }
        }
    }
}