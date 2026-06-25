using System;
using Droppy.Input;
using Droppy.InteractionSystem;
using Droppy.UI.ViewModel;
using UnityEngine;

namespace Droppy.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] protected DroppyInput input;
        [SerializeField] private InteractionAgent interactionAgent;
        [SerializeField] private PauseScreenViewModel pauseScreenViewModel;

        protected IDroppyInput DroppyInput;
        
        private void Awake()
        {
            SetDroppyInput(input);
        }

        protected virtual void OnEnable()
        {
            DroppyInput.OnInteractStarted += interactionAgent.StartInteraction;
            DroppyInput.OnInteractCanceled += interactionAgent.EndInteraction;
            DroppyInput.OnPauseStarted += TogglePause;
        }

        protected virtual void OnDisable()
        {
            DroppyInput.OnInteractStarted -= interactionAgent.StartInteraction;
            DroppyInput.OnInteractCanceled -= interactionAgent.EndInteraction;
            DroppyInput.OnPauseStarted -= TogglePause;
        }

        public void SetDroppyInput(IDroppyInput newDroppyInput)
        {
            DroppyInput = newDroppyInput;
        }

        private void TogglePause()
        {
            pauseScreenViewModel.RequestPause();
        }
    }
}