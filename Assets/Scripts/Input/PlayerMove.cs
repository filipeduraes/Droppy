using UnityEngine;
using UnityEngine.InputSystem;
namespace Droppy.Input
{

    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private DroppyControls controls; // referência pro input
        [SerializeField] private Vector2 moveInput;       // armazena o valor do input
        [SerializeField] private float speed = 5f;

        private void OnEnable()
        {
            controls.Player.Mover.performed += PerformMovement;
            controls.Player.Mover.canceled += StopMovement;
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Player.Mover.performed -= PerformMovement;
            controls.Player.Mover.canceled -= StopMovement;
            controls.Disable();
        }

        void Update()
        {

            Vector3 movement = new Vector3(moveInput.x, 0, 0);
            transform.Translate(movement * speed * Time.deltaTime);
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
