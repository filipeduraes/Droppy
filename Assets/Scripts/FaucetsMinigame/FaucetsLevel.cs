using Droppy.Input;
using Droppy.LevelSystem;
using Droppy.SpawnSystem;
using Droppy.StatSystem;
using UnityEngine;

namespace Droppy.FaucetsMinigame
{
    public class FaucetsLevel : Level
    {
        [SerializeField] private DroppyInput input;
        [SerializeField] private FaucetsManager faucetsManager;
        [SerializeField] private Spawner bucketSpawner;
        [SerializeField] private StatModifierTime timeModifier;

        private void Awake()
        {
            input.enabled = false;
            faucetsManager.enabled = false;
            timeModifier.enabled = false;
        }

        protected override void OnFinishIntroduction()
        {
            input.enabled = true;
            faucetsManager.enabled = true;
            timeModifier.enabled = true;
            
            bucketSpawner.StartSpawner();
        }

    }
}
