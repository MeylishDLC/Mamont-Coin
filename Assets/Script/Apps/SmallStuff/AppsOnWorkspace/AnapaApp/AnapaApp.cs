using UnityEngine;

namespace Script.Apps.SmallStuff.AppsOnWorkspace.AnapaApp
{
    public class AnapaApp: BasicWorkspaceApp
    {
        public Photo[] photos;
        [SerializeField] private PhotoApp photoApp;
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