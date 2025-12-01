using UnityEngine;

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
            
            input.OnMoveStarted += OnMovementStart;
            input.OnMoveCanceled += OnMovementEnd;
        }

        protected override void OnDisable()
        {
            input.OnMoveStarted -= OnMovementStart;
            input.OnMoveCanceled -= OnMovementEnd;

            base.OnDisable(); 
        }

        private void OnMovementStart()
        {
            
        }

        private void OnMovementEnd()
        {
            _horizontalInputValue = 0f;
        }
        
        private void ApplyHorizontalMovement()
        {
            if (!_rigidbody2D) return; 
            
            if (input.MoveInput.x != 0)
            {
                _horizontalInputValue = input.MoveInput.x;
            }

            Vector2 targetVelocity = new Vector2(
                _horizontalInputValue * movementSpeed, 
                _rigidbody2D.velocity.y
            );
            
            _rigidbody2D.velocity = targetVelocity;
        }
    }
}