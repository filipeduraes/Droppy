namespace Droppy.InteractionSystem
{
    public interface IInteractable
    {
        void EnterInteraction(InteractionAgent agent);
        void ExitInteraction(InteractionAgent agent);
        void StartInteraction(InteractionAgent agent);
        void EndInteraction(InteractionAgent agent);
    }
}