using UnityEngine;
using UnityEngine.InputSystem;

namespace Droppy.Input
{
    public class PlayerMove : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float speed = 5f;
        [SerializeField] private float screenPadding = 0.5f;
        
        private DroppyControls controls; 
        private Vector2 moveInput;
        private Camera mainCamera;
        private Rigidbody2D rb;

        private void Awake()
        {
            controls = new DroppyControls();
            mainCamera = Camera.main;
            
            rb = GetComponent<Rigidbody2D>();
            
            if (rb == null)
            {
                enabled = false;
            }
        }

        private void OnEnable()
        {
            controls.Player.Move.performed += PerformMovement;
            controls.Player.Move.canceled += StopMovement;
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Move.performed -= PerformMovement;
            controls.Player.Move.canceled -= StopMovement;
            controls.Disable();
        }

        private void Update()
        {
            ClampPosition();
        }
        
        private void FixedUpdate()
        {
            Move();
        }
        
        private void Move()
        {
            Vector2 targetVelocity = new Vector2(moveInput.x * speed, rb.velocity.y);
            rb.velocity = targetVelocity;
        }
        
        private void ClampPosition()
        {
            Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

            Vector3 currentPosition = transform.position;

            float minX = -screenBounds.x + screenPadding;
            float maxX = screenBounds.x - screenPadding;

            float clampedX = Mathf.Clamp(currentPosition.x, minX, maxX);
            
            transform.position = new Vector3(clampedX, currentPosition.y, currentPosition.z);
        }

        private void PerformMovement(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }

        private void StopMovement(InputAction.CallbackContext context)
        {
            moveInput = Vector2.zero;
        }
    }
}