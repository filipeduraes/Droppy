using System.Collections.Generic;
using UnityEngine;

namespace Droppy.InteractionSystem
{
    public class InteractionAgent : MonoBehaviour
    {
        private HashSet<IInteractableArea> currentInteractables = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractableArea interactable) && !currentInteractables.Contains(interactable))
            {
                interactable.EnterInteraction(gameObject);
                currentInteractables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractableArea interactable) && !currentInteractables.Contains(interactable))
            {
                interactable.ExitInteraction(gameObject);
                currentInteractables.Remove(interactable);
            }
        }

        public void StartInteraction()
        {
            foreach (IInteractableArea interactable in currentInteractables)
            {
                if (interactable is IHoldInteractable holdInteractable)
                {
                    holdInteractable.StartInteraction(gameObject);
                }
                
                if (interactable is IInteractable simpleInteractable)
                {
                    simpleInteractable.Interact(gameObject);
                }
            }
        }

        public void EndInteraction()
        {
            foreach (IInteractableArea interactable in currentInteractables)
            {
                if (interactable is IHoldInteractable holdInteractable)
                {
                    holdInteractable.EndInteraction(gameObject);
                }
            }
        }
    }
}