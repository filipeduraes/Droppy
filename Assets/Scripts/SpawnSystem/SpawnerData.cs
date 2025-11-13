using UnityEngine;
using System.Collections.Generic;

namespace Droppy.SpawnSystem
{
    [CreateAssetMenu(menuName = "Droppy/SpawnerData", fileName = "SpawnerData")]
    public class SpawnerData : ScriptableObject
    {
        [SerializeField] private List<Spawnable> spawnables = new();
        
        [SerializeField] private float minSpawnInterval = 1f;
        [SerializeField] private float maxSpawnInterval = 3f;
        [SerializeField] private float spawnGap = 0.5f;
        [SerializeField] private float lastSpawnTime;

        public List<Spawnable> Spawnables => spawnables;
        public float MinSpawnInterval => minSpawnInterval;
        public float MaxSpawnInterval => maxSpawnInterval;
        public float SpawnGap => spawnGap;

        public float LastSpawnTime => lastSpawnTime;
    }
}