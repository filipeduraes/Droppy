using System;
using Droppy.Input;
using Droppy.LevelSystem;
using Droppy.SpawnSystem;
using Droppy.VerticalGame;
using UnityEngine;

namespace Droppy.VerticalScrollerMinigame.LevelController
{
    public class VerticalScrollerLevel : Level
    {
        [Header("Vertical Scroller Level")]
        [SerializeField] private DroppyInput input;
        [SerializeField] private Spawner spawner;
        [SerializeField] private VerticalGameController controller;
        
        private ISpawner obstacleSpawner;
        private IDroppyInput droppyInput;

        protected override void Awake()
        {
            base.Awake();
            SetDroppyInput(input);
            SetObstacleSpawner(spawner);
        }

        private void Start()
        {
            Pause();
            controller.OnLevelFinished += Pause;
        }

        private void OnDestroy()
        {
            controller.OnLevelFinished -= Pause;
        }

        public void SetDroppyInput(IDroppyInput newInput)
        {
            droppyInput = newInput;
        }

        public void SetObstacleSpawner(ISpawner newSpawner)
        {
            obstacleSpawner = newSpawner;
        }

        public override void Resume()
        {
            base.Resume();
            droppyInput.Enable();
            obstacleSpawner.StartSpawner();
            controller.StartTimer();
        }
        
        public override void Pause()
        {
            base.Pause();
            droppyInput.Disable();
            obstacleSpawner.StopSpawner();
            controller.PauseTimer();
        }
    }
}