using System.Collections.Generic;
using UnityEngine;

namespace Droppy.InteractionSystem
{
    public class InteractionAgent : MonoBehaviour
    {
        private HashSet<IInteractable> currentInteractables = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable) && !currentInteractables.Contains(interactable))
            {
                interactable.EnterInteraction(this);
                currentInteractables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out IInteractable interactable) && !currentInteractables.Contains(interactable))
            {
                interactable.ExitInteraction(this);
                currentInteractables.Remove(interactable);
            }
        }

        public void StartInteraction()
        {
            foreach (IInteractable interactable in currentInteractables)
            {
                interactable.StartInteraction(this);
            }
        }

        public void EndInteraction()
        {
            foreach (IInteractable interactable in currentInteractables)
            {
                interactable.EndInteraction(this);
            }
        }
    }
}