using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Reflection;

namespace SpawningSystem
{



    [CreateAssetMenu(menuName = "Droppy/SpawnerData", fileName = "SpawnerData")]
    public class SpawnerData : ScriptableObject
    {
        [AssemblyTitle("Objetos que podem ser gerados")]
        [ListDrawerSettings(ShowIndexLabels = true, DraggableItems = true)]
        public List<Spawnable> spawnables = new();

        [AssemblyTitle("Pontos de Spawn")]
        [ListDrawerSettings(Expanded = true)]
        public List<Transform> spawnPoints = new();

        [AssemblyTitle("Configuração de Tempo")]
        [MinValue(0f)]
        public float minSpawnInterval = 1f;

        [MinValue(0f)]
        public float maxSpawnInterval = 3f;

        [MinValue(0f)]
        [Tooltip("Distância mínima entre spawns, se aplicável")]
        public float spawnGap = 0.5f;

        [HideInInspector]
        public float lastSpawnTime;
    }

}