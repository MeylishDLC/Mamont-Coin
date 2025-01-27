﻿using System;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.UI;

namespace Script.Core.Popups
{
    public class DuralingoPopup: Popup
    {
        [field: SerializeField] public int DuralingoCallsAmount { get; private set; }
        private Button button;
        public static event Action OnDuralingoCallClicked; 
        public override void OpenApp()
        {
            isOpen = true;
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
            
            gameObject.SetActive(true);

            if (AudioManager.HasMusic("Skype Call"))
            {
                AudioManager.StopMusic("Skype Call", STOP_MODE.IMMEDIATE);
            }
            AudioManager.InitializeMusic("Skype Call", FMODEvents.skypeCallSound);
        }

        private void OnClick() => OnDuralingoCallClicked?.Invoke();
        public override void CloseApp()
        {
            if (!isOpen)
            {
                return;
            }
            
            AudioManager.StopMusic("Skype Call", STOP_MODE.IMMEDIATE);
            Destroy(gameObject);
        }
    }
}