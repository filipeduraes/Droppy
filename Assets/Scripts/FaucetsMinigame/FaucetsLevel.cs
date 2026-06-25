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
            
            waterLevelController.OnLevelFinished += Pause;
            Pause();
        }

        private void OnDestroy()
        {
            waterLevelController.OnLevelFinished -= Pause;
        }

        public void SetDroppyInput(IDroppyInput newInput)
        {
            droppyInput = newInput;
        }

        public override void Resume()
        {
            base.Resume();
            droppyInput.Enable();
            faucetsManager.enabled = true;
            timeModifier.enabled = true;
            
            bucketSpawner.StartSpawner();
        }
        
        public override void Pause()
        {
            base.Pause();
            droppyInput.Disable();
            faucetsManager.enabled = false;
            timeModifier.enabled = false;
        }
    }
}
