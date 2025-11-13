using Droppy.Input;
using Droppy.InteractionSystem;
using UnityEngine;

namespace Droppy.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private DroppyInput input;
        [SerializeField] private InteractionAgent interactionAgent;
        
        private void OnEnable()
        {
            input.OnInteractStarted += interactionAgent.StartInteraction;
            input.OnInteractCanceled += interactionAgent.EndInteraction;
        }

        private void OnDisable()
        {
            input.OnInteractStarted -= interactionAgent.StartInteraction;
            input.OnInteractCanceled -= interactionAgent.EndInteraction;
        }
    }
}