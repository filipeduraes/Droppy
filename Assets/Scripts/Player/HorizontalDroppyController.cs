using UnityEngine;
using Droppy.ServiceLocatorSystem;

namespace Droppy.Player 
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class HorizontalDroppyController : PlayerController
    {
        [Header("Movement Settings")]
        [Tooltip("The maximum horizontal speed of the droplet (units per second).")]
        [SerializeField] private float movementSpeed = 7f;
        
        [Header("Screen Bounds")]
        [SerializeField] private float screenPadding = 0.5f;
        
        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 400f;
        
        private Rigidbody2D _rigidbody2D; 
        private Camera _mainCamera;

        protected void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            ServiceLocator.TryGetService(out _mainCamera);
        }

        protected override void OnEnable()
        {
            base.OnEnable(); 
            
            input.OnMoveStarted += OnMovementStart;
            input.OnMoveCanceled += OnMovementEnd;
            input.OnJumpStarted += OnJump;
        }

        protected override void OnDisable()
        {
            input.OnMoveStarted -= OnMovementStart;
            input.OnMoveCanceled -= OnMovementEnd;
            input.OnJumpStarted -= OnJump;

            base.OnDisable(); 
        }
        
        private void OnJump()
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void LateUpdate()
        {
            if (!_rigidbody2D) return;
            ClampPosition();
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
            if (_rigidbody2D.IsSleeping()) 
            {
                _rigidbody2D.WakeUp();
            }

            Vector2 targetVelocity = new Vector2(
                input.MoveInput.x * movementSpeed, 
                _rigidbody2D.velocity.y 
            );
            
            _rigidbody2D.velocity = targetVelocity;
        }
        
        private void ClampPosition()
        {
            Vector2 screenBounds = _mainCamera.ScreenToWorldPoint(
                new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z)
            );

            Vector3 currentPosition = transform.position;

            float minX = -screenBounds.x + screenPadding;
            float maxX = screenBounds.x - screenPadding;

            float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
            
            transform.position = new Vector3(clampedX, currentPosition.y, currentPosition.z);
        }
    }
}