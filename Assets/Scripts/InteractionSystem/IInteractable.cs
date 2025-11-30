using UnityEngine;

namespace Droppy.InteractionSystem
{
    public interface IInteractableArea : IEnterInteractableArea, IExitInteractableArea
    {
    }

    public interface IEnterInteractableArea
    {
        void EnterInteraction(GameObject agent);
    }
    
    public interface IExitInteractableArea
    {
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
