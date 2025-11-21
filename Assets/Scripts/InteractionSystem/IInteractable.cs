using UnityEngine;

namespace Droppy.InteractionSystem
{
    public interface IInteractableArea
    {
        void EnterInteraction(GameObject agent);
        void ExitInteraction(GameObject agent);
    }

    public interface IHoldInteractable
    {
        void StartInteraction(GameObject agent);
        void EndInteraction(GameObject agent);
    }

    public interface IInteractable
    {
        void Interact(GameObject agent);
    }
}
