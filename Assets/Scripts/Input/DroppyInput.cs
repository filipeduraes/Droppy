using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Droppy.Input
{
    public class DroppyInput : MonoBehaviour
    {
        public event Action<Vector2> OnPointerStarted = delegate { };
        
        public event Action OnMoveStarted = delegate { };
        public event Action OnMoveCanceled = delegate { };
        
        public event Action OnJumpStarted = delegate { };
        
        public event Action OnInteractStarted = delegate { };
        public event Action OnInteractCanceled = delegate { };
        
        public Vector2 MoveInput => controls.Player.Move.ReadValue<Vector2>();
        
        public DroppyControls Controls => controls; 
        
        private DroppyControls controls;

        private void Awake()
        {
            controls = new DroppyControls();
        }

        private void OnDestroy()
        {
            controls.Dispose();
        }

        private void OnEnable()
        {
            controls.Player.Move.started += SendMoveStarted;
            controls.Player.Move.canceled += SendMoveCanceled;
            
            controls.Player.Jump.started += SendJumpStarted;
            
            controls.Player.Interact.started += SendInteractStarted;
            controls.Player.Interact.canceled += SendInteractCanceled;

            controls.Player.Pointer.started += SendPointerStarted;
            
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Move.started -= SendMoveStarted;
            controls.Player.Move.canceled -= SendMoveCanceled;
            
            controls.Player.Jump.started -= SendJumpStarted;
            
            controls.Player.Interact.started -= SendInteractStarted;
            controls.Player.Interact.canceled -= SendInteractCanceled;
            
            controls.Player.Pointer.started -= SendPointerStarted;
            
            controls.Disable();
        }


        private void SendMoveStarted(InputAction.CallbackContext context)
        {
            OnMoveStarted();
        }
        
        private void SendMoveCanceled(InputAction.CallbackContext context)
        {
            OnMoveCanceled();
        }
        
        private void SendJumpStarted(InputAction.CallbackContext context)
        {
            OnJumpStarted();
        }

        
        private void SendInteractStarted(InputAction.CallbackContext context)
        {
            OnInteractStarted();
        }
        
        private void SendInteractCanceled(InputAction.CallbackContext context)
        {
            OnInteractCanceled();
        }
        
        
        private void SendPointerStarted(InputAction.CallbackContext context)
        {
            Vector2 position = controls.Player.PointerPosition.ReadValue<Vector2>();
            OnPointerStarted(position);
        }
    }
}