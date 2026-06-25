using Droppy.Input;
using Droppy.PieceMinigame.Runtime;
using UnityEngine;

namespace Droppy.PieceMinigame.Level
{
    public class PieceMinigameLevel : LevelSystem.Level
    {
        [SerializeField] private FlowController flowController;
        [SerializeField] private DroppyInput droppyInput;

        private IFlowController controller;
        private IDroppyInput input;

        protected override void Awake()
        {
            base.Awake();
            SetFlowController(flowController);
            SetInput(droppyInput);
        }

        private void Start()
        {
            input.Disable();
        }

        public void SetFlowController(IFlowController newFlowController)
        {
            controller = newFlowController;
        }

        public void SetInput(IDroppyInput newInput)
        {
            input = newInput;
        }

        public override void Resume()
        {
            base.Resume();
            input.Enable();
            controller.ResumeFlow();
        }

        public override void Pause()
        {
            base.Pause();
            input.Disable();
            controller.PauseFlow();
        }
    }
}