using Droppy.Input;
using Droppy.LevelSystem;
using Droppy.SpawnSystem;
using Droppy.StatSystem;
using Droppy.WaterLevel;
using UnityEngine;

namespace Droppy.FaucetsMinigame
{
    public class FaucetsLevel : Level
    {
        [Header("Faucets Level")]
        [SerializeField] private DroppyInput input;
        [SerializeField] private FaucetsManager faucetsManager;
        [SerializeField] private WaterLevelController waterLevelController;
        [SerializeField] private Spawner bucketSpawner;
        [SerializeField] private StatModifierTime timeModifier;

        private void Awake()
        {
            waterLevelController.OnLevelFinished += StopLevel;
            StopLevel();
        }

        private void OnDestroy()
        {
            waterLevelController.OnLevelFinished -= StopLevel;
        }

        protected override void OnFinishIntroduction()
        {
            input.enabled = true;
            faucetsManager.enabled = true;
            timeModifier.enabled = true;
            
            bucketSpawner.StartSpawner();
        }
        
        private void StopLevel()
        {
            input.enabled = false;
            faucetsManager.enabled = false;
            timeModifier.enabled = false;
        }
    }
}
