using UnityEngine;
using Droppy.Input;
using Droppy.InteractionSystem;
using Droppy.ServiceLocatorSystem;

namespace Droppy.Player
{
    public class PiecePlayerController : MonoBehaviour 
    {
        [SerializeField] private DroppyInput input;
        
        private Camera mainCamera;
        private IDroppyInput droppyInput;

        private void Awake()
        {
            SetDroppyInput(input);
        }

        private void Start()
        {
            ServiceLocator.TryGetService(out mainCamera);
        }

        private void OnEnable()
        {
            droppyInput.OnPointerStarted += TryInteract;
        }

        private void OnDisable()
        {
            droppyInput.OnPointerStarted -= TryInteract;
        }

        public void SetDroppyInput(IDroppyInput newInput)
        {
            droppyInput = newInput;
        }

        private void TryInteract(Vector2 pointerPosition)
        {
            if (mainCamera == null)
            {
                return;
            }
            
            Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit && hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(gameObject);
            }
        }
    }
}