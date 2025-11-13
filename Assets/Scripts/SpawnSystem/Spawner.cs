using UnityEngine;
using System.Collections.Generic;

namespace Droppy.SpawnSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData data;
        [SerializeField] private List<Transform> spawnPoints = new();

        private void Update() 
        {
            if (Time.time > data.LastSpawnTime)
            {
                SpawnObject();
            }
        }

        private void SpawnObject()
        {
            GameObject prefabToSpawn = ChooseSpawnable();
            if (prefabToSpawn == null)
            {
                Debug.LogWarning("Não foi possível escolher um prefab. A lista de Spawnables está vazia?", this);
                return;
            }

            Transform spawnPoint = ChooseSpawnPoint();
            if (spawnPoint == null)
            {
                Debug.LogWarning("Não foi possível escolher um ponto de spawn. A lista spawnPoints está vazia?", this);
                return;
            }

            Instantiate(prefabToSpawn, spawnPoint.position, spawnPoint.rotation);

            float interval = Random.Range(data.MinSpawnInterval, data.MaxSpawnInterval);
            data.LastSpawnTime = Time.time + interval;
        }

        private GameObject ChooseSpawnable()
        {
            if (data.Spawnables == null || data.Spawnables.Count == 0)
                return null;

            float totalWeight = 0;
            foreach (Spawnable spawnable in data.Spawnables)
            {
                totalWeight += spawnable.Weight;
            }

            float randomValue = Random.Range(0, totalWeight);

            foreach (Spawnable spawnable in data.Spawnables)
            {
                if (randomValue < spawnable.Weight)
                {
                    return spawnable.Prefab;
                }
                randomValue -= spawnable.Weight;
            }

            return data.Spawnables[data.Spawnables.Count - 1].Prefab;
        }

        private Transform ChooseSpawnPoint()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
                return null;

            int index = Random.Range(0, spawnPoints.Count);
            return spawnPoints[index];
        }
    }
}