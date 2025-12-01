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
        
        protected void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
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
            ApplyHorizontalMovement();
        }

        private void OnMovementEnd()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
        
        private void ApplyHorizontalMovement()
        {
            Vector2 targetVelocity = new Vector2(
                input.MoveInput.x * movementSpeed, 
                _rigidbody2D.velocity.y 
            );
            
            _rigidbody2D.velocity = targetVelocity;
        }
    }
}