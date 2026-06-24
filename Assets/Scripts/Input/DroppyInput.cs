using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Droppy.Input
{
    public interface IDroppyInput
    {
        event Action<Vector2> OnPointerStarted;
        event Action OnMoveStarted;
        event Action OnMoveCanceled;
        
        event Action OnJumpStarted;
        event Action OnJumpCanceled;
        
        event Action OnInteractStarted;
        event Action OnInteractCanceled;
        
        Vector2 MoveInput { get; }
        
        void Enable();
        void Disable();
        
        void SendMoveStarted();
        void SendMoveCanceled();
        void SendJumpStarted();
        void SendJumpCanceled();
        void SendInteractStarted();
        void SendInteractCanceled();
        void SendPointerStarted(Vector2 position);
    }
    
    public class DroppyInput : MonoBehaviour, IDroppyInput
    {
        public event Action<Vector2> OnPointerStarted = delegate { };
        
        public event Action OnMoveStarted = delegate { };
        public event Action OnMoveCanceled = delegate { };
        
        public event Action OnJumpStarted = delegate { };
        public event Action OnJumpCanceled = delegate { };
        
        public event Action OnInteractStarted = delegate { };
        public event Action OnInteractCanceled = delegate { };
        
        public Vector2 MoveInput => controls.Player.Move.ReadValue<Vector2>();

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
            controls.Player.Jump.canceled += SendJumpCanceled;
            
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
            controls.Player.Jump.canceled -= SendJumpCanceled;
            
            controls.Player.Interact.started -= SendInteractStarted;
            controls.Player.Interact.canceled -= SendInteractCanceled;
            
            controls.Player.Pointer.started -= SendPointerStarted;
            
            controls.Disable();
        }
        
        public void Enable()
        {
            enabled = true;
        }

        public void Disable()
        {
            enabled = false;
        }
        
        public void SendMoveStarted()
        {
            OnMoveStarted();
        }

        public void SendMoveCanceled()
        {
            OnMoveCanceled();
        }

        public void SendJumpStarted()
        {
            OnJumpStarted();
        }

        public void SendJumpCanceled()
        {
            OnJumpCanceled();
        }

        public void SendInteractStarted()
        {
            OnInteractStarted();
        }

        public void SendInteractCanceled()
        {
            OnInteractCanceled();
        }

        public void SendPointerStarted(Vector2 position)
        {
            OnPointerStarted(position);
        }


        private void SendMoveStarted(InputAction.CallbackContext context) => SendMoveStarted();

        private void SendMoveCanceled(InputAction.CallbackContext context) => SendMoveCanceled();


        private void SendJumpStarted(InputAction.CallbackContext context) => SendJumpStarted();

        private void SendJumpCanceled(InputAction.CallbackContext context) => SendJumpCanceled();


        private void SendInteractStarted(InputAction.CallbackContext context) => SendInteractStarted();

        private void SendInteractCanceled(InputAction.CallbackContext context) => SendInteractCanceled();


        private void SendPointerStarted(InputAction.CallbackContext context)
        {
            Vector2 position = controls.Player.PointerPosition.ReadValue<Vector2>();
            SendPointerStarted(position);
        }
    }
}