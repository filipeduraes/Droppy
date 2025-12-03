using UnityEngine;
using Droppy.ServiceLocatorSystem;

namespace Droppy.Player 
{
    [RequireComponent(typeof(Rigidbody2D))] 
    public class HorizontalDroppyController : PlayerController
    {
        [Header("Movement Settings")]
        [Tooltip("The maximum horizontal speed of the droplet (unidades por segundo).")]
        [SerializeField] private float movementSpeed = 7f;
        
        [Header("Screen Bounds")]
        [SerializeField] private float screenPadding = 0.5f;
        
        [Header("Jump Settings")]
        [Tooltip("If true, enables jumping and ground checking logic.")]
        [SerializeField] private bool jumpEnabled = true;
        [SerializeField] private float jumpForce = 400f;
        
        [Header("Ground Check")]
        [Tooltip("A LayerMask que define o que é considerado chão.")]
        [SerializeField] private LayerMask groundLayer;
        [Tooltip("Distância máxima que o raycast irá percorrer para checar o chão.")]
        [SerializeField] private float groundCheckDistance = 0.1f;
        
        private Rigidbody2D _rigidbody2D; 
        private Camera _mainCamera;
        private bool _isGrounded = false;
        private float _currentMoveDirection = 0f; 

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
            
            if (jumpEnabled)
            {
                input.OnJumpStarted += OnJump;
            }
        }

        protected override void OnDisable()
        {
            input.OnMoveStarted -= OnMovementStart;
            input.OnMoveCanceled -= OnMovementEnd;
            
            if (jumpEnabled)
            {
                input.OnJumpStarted -= OnJump;
            }

            base.OnDisable(); 
        }
        
        private void FixedUpdate() 
        {
            if (jumpEnabled)
            {
                CheckIfGrounded();
            }
            
            HandleHorizontalMovement(); 
        }

        private void LateUpdate()
        {
            if (!_mainCamera) return;
            ClampPosition(); 
        }

        private void OnMovementStart()
        {
            _currentMoveDirection = input.MoveInput.x;
        }

        private void OnMovementEnd()
        {
            _currentMoveDirection = 0f;
        }

        private void OnJump()
        {
            if (_isGrounded)
            {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0f);
        
                _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
        }
        
        private void HandleHorizontalMovement()
        {
            if (_rigidbody2D.IsSleeping()) 
            {
                _rigidbody2D.WakeUp();
            }

            Vector2 targetVelocity = new Vector2(
                _currentMoveDirection * movementSpeed, 
                _rigidbody2D.velocity.y 
            );
            
            _rigidbody2D.velocity = targetVelocity;
        }

        private void CheckIfGrounded()
        {
            Vector2 origin = transform.position;
            
            RaycastHit2D hit = Physics2D.Raycast(
                origin, 
                Vector2.down, 
                groundCheckDistance, 
                groundLayer
            );
            
            _isGrounded = hit.collider;
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