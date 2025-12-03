using UnityEngine;
using Droppy.ServiceLocatorSystem;

namespace Droppy.Player 
{
    public enum PlayerType
    {
        VerticalScroller,
        Platformer
    }
    
    public class HorizontalDroppyController : PlayerController
    {
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 7f;
        [SerializeField] private PlayerType playerType = PlayerType.VerticalScroller;
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;
        
        [Header("Screen Bounds")]
        [SerializeField] private float screenPadding = 0.5f;
        
        [Header("Platformer Settings")]
        [SerializeField] private float jumpVelocity = 1.0f;
        [SerializeField] private float fallGravity = 3.0f;
        [SerializeField] private float jumpGravity = 1.0f;
        [SerializeField] private float walkGravity = 0.0f;
        [SerializeField] private float minJumpTime = 0.2f;
        
        [Header("Ground Check")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float groundCheckDistance = 0.1f;
        
        private Camera mainCamera;
        private bool isGrounded = false;
        private bool isFalling = false;
        private bool jumpCanceled = false;
        private float timeFromLastJump = 0.0f;
        private float currentMoveDirection = 0f;
        
        private static readonly int IsMovingParameter = Animator.StringToHash("IsMoving");
        private static readonly int XDirectionParameter = Animator.StringToHash("XDirection");

        private void Start()
        {
            ServiceLocator.TryGetService(out mainCamera);
        }

        protected override void OnEnable()
        {
            base.OnEnable(); 
            
            input.OnMoveStarted += OnMovementStart;
            input.OnMoveCanceled += OnMovementEnd;
            
            if (playerType == PlayerType.Platformer)
            {
                input.OnJumpStarted += OnJump;
                input.OnJumpCanceled += CancelJump;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable(); 
            
            input.OnMoveStarted -= OnMovementStart;
            input.OnMoveCanceled -= OnMovementEnd;
            
            if (playerType == PlayerType.Platformer)
            {
                input.OnJumpStarted -= OnJump;
                input.OnJumpCanceled -= CancelJump;
            }
        }

        private void FixedUpdate() 
        {
            if (playerType == PlayerType.Platformer)
            {
                CheckIfGrounded();

                bool isJumping = !isGrounded && !isFalling;
                bool jumpWasCanceled = timeFromLastJump >= minJumpTime && jumpCanceled;
                bool isStartingToFall = body.velocity.y < 0;
                bool canApplyFallGravity = isJumping && (jumpWasCanceled || isStartingToFall);
                bool finishedFall = isFalling && isGrounded;
                
                if (canApplyFallGravity)
                {
                    isFalling = true;
                    body.gravityScale = fallGravity;
                }

                if (finishedFall)
                {
                    isFalling = false;
                    body.gravityScale = walkGravity;
                }
            }
            else if (playerType == PlayerType.VerticalScroller)
            {
                ClampPosition(); 
            }
            
            HandleHorizontalMovement(); 
        }

        private void OnMovementStart()
        {
            currentMoveDirection = input.MoveInput.x;
        }

        private void OnMovementEnd()
        {
            currentMoveDirection = 0f;
        }

        private void OnJump()
        {
            if (isGrounded)
            {
                Vector2 velocity = body.velocity;
                velocity.y = jumpVelocity;
                body.velocity = velocity;
                body.gravityScale = jumpGravity;

                timeFromLastJump = Time.time;
            }
        }
        
        private void CancelJump()
        {
            if (!isFalling && !isGrounded)
            {
                if (timeFromLastJump < minJumpTime)
                {
                    jumpCanceled = true;
                    return;
                }
                
                Vector2 velocity = body.velocity;
                velocity.y = 0.0f;
                body.velocity = velocity;
                body.gravityScale = fallGravity;
            }
        }
        
        private void HandleHorizontalMovement()
        {
            Vector2 targetVelocity = body.velocity;
            targetVelocity.x = currentMoveDirection * movementSpeed;
            
            body.velocity = targetVelocity;

            if (playerType == PlayerType.Platformer)
            {
                animator.SetBool(IsMovingParameter, body.velocity.x != 0 && isGrounded);

                if (body.velocity.x != 0)
                {
                    animator.SetFloat(XDirectionParameter, body.velocity.x);
                }
            }
        }

        private void CheckIfGrounded()
        {
            Vector2 origin = transform.position;
            
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
            
            isGrounded = hit.collider;
        }
        
        private void ClampPosition()
        {
            Vector3 screenBoundsPosition = new(Screen.width, Screen.height, mainCamera.transform.position.z);
            Vector2 screenBounds = mainCamera.ScreenToWorldPoint(screenBoundsPosition);

            Vector3 currentPosition = transform.position;

            float minX = mainCamera.transform.position.x + -screenBounds.x + screenPadding;
            float maxX = mainCamera.transform.position.x + screenBounds.x - screenPadding;

            float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
            
            transform.position = new Vector3(clampedX, currentPosition.y, currentPosition.z);
        }
    }
}