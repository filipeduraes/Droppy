using UnityEngine;
using System.Collections.Generic;
using IdeaToGame.ObjectPooling;


namespace Droppy.SpawnSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData data;
        [SerializeField] private List<Transform> spawnPoints = new();

        private float lastSpawnTime;
        private float totalWeight;

        private void Awake()
        {
            totalWeight = 0;
            if (data.Spawnables != null)
            {
                foreach (Spawnable spawnable in data.Spawnables)
                {
                    totalWeight += spawnable.Weight;
                }
            }
        }

        private void Update()
        {
            if (Time.time > lastSpawnTime)
            {
                SpawnObject();
            }
        }

        private void SpawnObject()
        {
            GameObject prefabToSpawn = ChooseSpawnable();
            if (prefabToSpawn == null)
            {
                if (totalWeight <= 0)
                {
                    Debug.LogWarning("Não é possível spawnar. Lista de Spawnables está vazia ou o peso total é zero.", this);
                    enabled = false;
                    return;
                }
                Debug.LogWarning("Não foi possível escolher um prefab. (ChooseSpawnable retornou null)", this);
                return;
            }

            Transform spawnPoint = ChooseSpawnPoint();
            if (spawnPoint == null)
            {
                Debug.LogWarning("Não foi possível escolher um ponto de spawn. A lista spawnPoints está vazia?", this);
                return;
            }

            Transform spawnedTransform = ObjectPool.GetFromPool(prefabToSpawn.transform);
            if (spawnedTransform != null)
            {
                spawnedTransform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            }
            else
            {
                Debug.LogWarning($"Pool não conseguiu fornecer um objeto para o prefab: {prefabToSpawn.name}", this);
                return; 
            }

            float interval = Random.Range(data.MinSpawnInterval, data.MaxSpawnInterval);
            lastSpawnTime = Time.time + interval;
        }
        private GameObject ChooseSpawnable()
        {
            if (totalWeight <= 0)
                return null;

            float randomValue = Random.Range(0, totalWeight);

            foreach (Spawnable spawnable in data.Spawnables)
            {
                if (randomValue < spawnable.Weight)
                {
                    return spawnable.Prefab;
                }
                randomValue -= spawnable.Weight;
            }

            if (data.Spawnables.Count > 0)
                return data.Spawnables[data.Spawnables.Count - 1].Prefab;

            return null;
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