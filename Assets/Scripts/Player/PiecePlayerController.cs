using System;
using UnityEngine;
using Droppy.Input;
using Droppy.InteractionSystem;

namespace Droppy.Player
{
    public class PiecePlayerController : MonoBehaviour 
    {
        [SerializeField] private DroppyInput input;
        [SerializeField] private Camera mainCamera;

        private void OnEnable()
        {
            input.OnPointerStarted += TryInteract;
        }

        private void OnDisable()
        {
            input.OnPointerStarted -= TryInteract;
        }

        private void TryInteract(Vector2 pointerPosition)
        {
            Ray ray = mainCamera.ScreenPointToRay(pointerPosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit && hit.transform.TryGetComponent(out IInteractable interactable))
            {
                interactable.Interact(gameObject);
            }
        }
    }
}