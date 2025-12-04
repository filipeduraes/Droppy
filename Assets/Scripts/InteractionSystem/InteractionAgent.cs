using System.Collections.Generic;
using UnityEngine;

namespace Droppy.InteractionSystem
{
    public class InteractionAgent : MonoBehaviour
    {
        private readonly HashSet<GameObject> currentInteractables = new();
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            currentInteractables.Add(other.gameObject);
            IEnterInteractableArea[] enterInteractables = other.GetComponents<IEnterInteractableArea>();

            foreach (IEnterInteractableArea enterInteractable in enterInteractables)
            {
                enterInteractable.EnterInteraction(gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (currentInteractables.Contains(other.gameObject))
            {
                IExitInteractableArea[] exitInteractables = other.GetComponents<IExitInteractableArea>();

                foreach (IExitInteractableArea enterInteractable in exitInteractables)
                {
                    enterInteractable.ExitInteraction(gameObject);
                }
                
                currentInteractables.Remove(other.gameObject);
            }
        }

        public void StartInteraction()
        {
            foreach (GameObject interactableObject in currentInteractables)
            {
                IHoldInteractable[] holdInteractables = interactableObject.GetComponents<IHoldInteractable>();
                IInteractable[] interactables = interactableObject.GetComponents<IInteractable>();

                foreach (IHoldInteractable holdInteractable in holdInteractables)
                {
                    holdInteractable.StartInteraction(gameObject);
                }
                
                foreach (IInteractable interactable in interactables)
                {
                    interactable.Interact(gameObject);
                }
            }
        }

        public void EndInteraction()
        {
            foreach (GameObject interactableObject in currentInteractables)
            {
                IHoldInteractable[] holdInteractables = interactableObject.GetComponents<IHoldInteractable>();

                foreach (IHoldInteractable holdInteractable in holdInteractables)
                {
                    holdInteractable.EndInteraction(gameObject);
                }
            }
        }
    }
}