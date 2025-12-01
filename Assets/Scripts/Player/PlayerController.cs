using Droppy.Input;
using Droppy.InteractionSystem;
using UnityEngine;

namespace Droppy.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] protected DroppyInput input;
        [SerializeField] private InteractionAgent interactionAgent;
        
        protected virtual void OnEnable()
        {
            input.OnInteractStarted += interactionAgent.StartInteraction;
            input.OnInteractCanceled += interactionAgent.EndInteraction;
        }

        protected virtual void OnDisable()
        {
            input.OnInteractStarted -= interactionAgent.StartInteraction;
            input.OnInteractCanceled -= interactionAgent.EndInteraction;
        }
    }
}