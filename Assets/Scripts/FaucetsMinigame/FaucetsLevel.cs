using Droppy.Input;
using Droppy.LevelSystem;
using Droppy.FaucetsMinigame;
using Droppy.SpawnSystem;
using UnityEngine;

namespace Droppy.FaucetsMinigame
{
    public class FaucetsLevel : Level
    {
        [Header("Fase 02 - Torneiras")]
        [SerializeField] private DroppyInput input;
        [SerializeField] private FaucetsManager faucetsManager;
        [SerializeField] private Spawner bucketSpawner;

        private void Awake()
        {
            input.enabled = false;
            faucetsManager.enabled = false;

            if (bucketSpawner != null)
                bucketSpawner.StopSpawner();
        }

        protected override void OnFinishIntroduction()
        {
            input.enabled = true;

            faucetsManager.gameObject.SetActive(true);

            if (bucketSpawner != null)
                bucketSpawner.StartSpawner();
        }

    }
}
