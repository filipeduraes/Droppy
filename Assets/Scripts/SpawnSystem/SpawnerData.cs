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
    }
}