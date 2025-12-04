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

        private void Awake()
        {
            StopLevel();
            controller.OnLevelFinished += StopLevel;
        }

        private void OnDestroy()
        {
            controller.OnLevelFinished -= StopLevel;
        }

        protected override void OnFinishIntroduction()
        {
            input.enabled = true;
            spawner.StartSpawner();
            controller.StartTimer();
        }
        
        private void StopLevel()
        {
            input.enabled = false;
            spawner.StopSpawner();
        }
    }
}