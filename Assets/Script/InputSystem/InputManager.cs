using Script.Managers;
using Script.Sound;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script.InputSystem
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputManager : MonoBehaviour
    {
        private Vector2 pointerPos = Vector2.zero;
    
        private bool mouseButtonPressed;
        private bool keyboardButtonPressed;
        private bool isDragging;

        public static InputManager instance { get; private set; }

        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Found more than one Input Manager in the scene.");
            }
            instance = this;
        }

        public void MouseButtonPressed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                mouseButtonPressed = true;
                AudioManager.instance.PlayOneShot(FMODEvents.instance.clickSound);
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