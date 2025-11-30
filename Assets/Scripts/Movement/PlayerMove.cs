using UnityEngine;
using UnityEngine.InputSystem;
using Droppy.ServiceLocatorSystem;
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

        private void Awake()
        {
            controls = new DroppyControls();
            
        }
        private void Start()
        {
            ServiceLocator.TryGetService(out mainCamera);
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
            Move();
            if (mainCamera != null)
            {
                ClampPosition();
            }
        }
        
        private void Move()
        {
            Vector3 movement = new(moveInput.x, 0, 0);
            transform.Translate(movement * (speed * Time.deltaTime));
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