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

        private IDroppyInput droppyInput;
        
        protected override void Awake()
        {
            base.Awake();
            SetDroppyInput(input);
            
            waterLevelController.OnLevelFinished += StopLevel;
            StopLevel();
        }

        private void OnDestroy()
        {
            waterLevelController.OnLevelFinished -= StopLevel;
        }

        public void SetDroppyInput(IDroppyInput newInput)
        {
            droppyInput = newInput;
        }

        protected override void OnFinishIntroduction()
        {
            droppyInput.Enable();
            faucetsManager.enabled = true;
            timeModifier.enabled = true;
            
            bucketSpawner.StartSpawner();
        }
        
        private void StopLevel()
        {
            droppyInput.Disable();
            faucetsManager.enabled = false;
            timeModifier.enabled = false;
        }
    }
}
