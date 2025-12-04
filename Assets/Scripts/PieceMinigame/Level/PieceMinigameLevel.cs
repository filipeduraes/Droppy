using System;
using Droppy.Input;
using Droppy.PieceMinigame.Runtime;
using UnityEngine;

namespace Droppy.PieceMinigame.Level
{
    public class PieceMinigameLevel : LevelSystem.Level
    {
        [SerializeField] private FlowController flowController;
        [SerializeField] private DroppyInput droppyInput;

        private void Awake()
        {
            droppyInput.enabled = false;
        }

        protected override void OnFinishIntroduction()
        {
            droppyInput.enabled = true;
            flowController.StartFlow();
        }
    }
}