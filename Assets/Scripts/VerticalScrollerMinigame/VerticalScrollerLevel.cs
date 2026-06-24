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
            StopLevel();
            controller.OnLevelFinished += StopLevel;
        }

        private void OnDestroy()
        {
            controller.OnLevelFinished -= StopLevel;
        }

        public void SetDroppyInput(IDroppyInput newInput)
        {
            droppyInput = newInput;
        }

        public void SetObstacleSpawner(ISpawner newSpawner)
        {
            obstacleSpawner = newSpawner;
        }

        protected override void OnFinishIntroduction()
        {
            droppyInput.Enable();
            obstacleSpawner.StartSpawner();
            controller.StartTimer();
        }
        
        private void StopLevel()
        {
            droppyInput.Disable();
            obstacleSpawner.StopSpawner();
        }
    }
}