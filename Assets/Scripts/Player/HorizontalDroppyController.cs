using UnityEngine;
using UnityEngine.InputSystem; 

namespace Droppy.Player 
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class HorizontalDroppyController : PlayerController
    {
        [Header("Movement Settings")]
        [Tooltip("The maximum horizontal speed of the droplet (units per second).")]
        [SerializeField] private float movementSpeed = 7f;
        
        private Rigidbody2D _rigidbody2D; 
        private float _horizontalInputValue; 
        
        protected void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            ApplyHorizontalMovement();
        }
        
        protected override void OnEnable()
        {
            base.OnEnable(); 
            
            input.Controls.Player.Move.performed += ReadMovementInput;
            input.Controls.Player.Move.canceled += ReadMovementInput;
        }

        protected override void OnDisable()
        {
            input.Controls.Player.Move.performed -= ReadMovementInput;
            input.Controls.Player.Move.canceled -= ReadMovementInput;

            base.OnDisable(); 
        }
        
        
        private void ReadMovementInput(InputAction.CallbackContext context)
        {
            _horizontalInputValue = context.ReadValue<Vector2>().x;
        }

        private void ApplyHorizontalMovement()
        {
            if (!_rigidbody2D) return; 

            Vector2 targetVelocity = new Vector2(
                _horizontalInputValue * movementSpeed, 
                _rigidbody2D.velocity.y
            );
            
            _rigidbody2D.velocity = targetVelocity;
        }
    }
}