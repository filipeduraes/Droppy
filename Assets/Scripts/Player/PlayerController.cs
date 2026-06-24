using System;
using Droppy.Input;
using Droppy.InteractionSystem;
using UnityEngine;

namespace Droppy.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] protected DroppyInput input;
        [SerializeField] private InteractionAgent interactionAgent;

        protected IDroppyInput DroppyInput;
        
        private void Awake()
        {
            SetDroppyInput(input);
        }

        protected virtual void OnEnable()
        {
            DroppyInput.OnInteractStarted += interactionAgent.StartInteraction;
            DroppyInput.OnInteractCanceled += interactionAgent.EndInteraction;
        }

        protected virtual void OnDisable()
        {
            DroppyInput.OnInteractStarted -= interactionAgent.StartInteraction;
            DroppyInput.OnInteractCanceled -= interactionAgent.EndInteraction;
        }

        public void SetDroppyInput(IDroppyInput newDroppyInput)
        {
            DroppyInput = newDroppyInput;
        }
    }
}