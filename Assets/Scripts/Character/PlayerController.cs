using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private PlayableCharacter _playableCharacter;

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            _playableCharacter.RequestMove(movement);
        }
        
        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 look = context.ReadValue<Vector2>();
            _playableCharacter.RequestLook(look);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _playableCharacter.RequestJump();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            // if (context.performed)
            // {
            //     _playableCharacter.RequestSprint(true);
            // }
            //
            // if (context.canceled)
            // {
            //     _playableCharacter.RequestSprint(false);
            // }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // _playableCharacter.RequestInteract();
            }
        }

        public void OnFlashlightToggle(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // _playableCharacter.RequestFlashlight();
            }
        }

        private void OnPickUp(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // _playableCharacter.RequestPickUp();
            }
        }

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            if (!_playerInput)
            {
                Debug.LogError("No player input found");
            }
            
            _playableCharacter = GetComponent<PlayableCharacter>();
            if (!_playableCharacter)
            {
                Debug.LogError("No character found");
            }
        }

        private void Start()
        {
            InputActionMap map = _playerInput.actions.FindActionMap("Game");
            
            if (map != null)
            {
                map.Enable();
            }
            else
            {
                Debug.LogError("Action Map 'Game' not found!");
            }
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}