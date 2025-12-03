using Droppy.Input;
using Droppy.LevelSystem;
using Droppy.SpawnSystem;
using UnityEngine;

namespace Droppy.VerticalScrollerMinigame.LevelController
{
    public class VerticalScrollerLevel : Level
    {
        [Header("Vertical Scroller Level")]
        [SerializeField] private DroppyInput input;
        [SerializeField] private Spawner spawner;

        private void Awake()
        {
            input.enabled = false;
            spawner.StopSpawner();
        }

        protected override void OnFinishIntroduction()
        {
            input.enabled = true;
            spawner.StartSpawner();
        }
    }
}