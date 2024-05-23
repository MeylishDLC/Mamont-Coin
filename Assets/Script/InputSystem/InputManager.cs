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
        private Vector2 pointerPos = Vector2.zero;
    
        private bool mouseButtonPressed;
        private bool keyboardButtonPressed;
        private bool isDragging;

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

        public void KeyboardButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                keyboardButtonPressed = true;
            }
            else if (context.canceled)
            {
                keyboardButtonPressed = false;
            }
        }
        
        public bool GetMouseButtonPressed()
        {
            bool result = mouseButtonPressed;
            mouseButtonPressed = false;
            return result;
        }
        public bool GetKeyboardButtonPressed()
        {
            bool result = keyboardButtonPressed;
            keyboardButtonPressed = false;
            return result;
        }

    }
}