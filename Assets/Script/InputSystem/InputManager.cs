using Script.Managers;
using Script.Sound;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Script.InputSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        private bool mouseButtonPressed;
        private AudioManager audioManager;
        private FMODEvents FMODEvents;
        
        [Inject]
        public void Construct(AudioManager audioManager, FMODEvents fmodEvents)
        {
            this.audioManager = audioManager;
            FMODEvents = fmodEvents;
        }

        public void MouseButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                mouseButtonPressed = true;
                audioManager.PlayOneShot(FMODEvents.clickSound);
            }
            else if (context.canceled)
            {
                mouseButtonPressed = false;
            }
        }

    }
}