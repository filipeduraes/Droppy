using UnityEngine;
using System.Collections.Generic;
using IdeaToGame.ObjectPooling;


namespace Droppy.SpawnSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData data;
        [SerializeField] private List<Transform> spawnPoints = new();
        [SerializeField] private bool destroyAllOnDisable = true;

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

        private void OnDisable()
        {
            if (!destroyAllOnDisable)
            {
                return;
            }

            ObjectPool.DestroyAllPooledObjects();
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
            Transform spawnPoint = ChooseSpawnPoint();
            Transform spawnedTransform = ObjectPool.GetFromPool(prefabToSpawn.transform);

            spawnedTransform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            
            float interval = Random.Range(data.MinSpawnInterval, data.MaxSpawnInterval);
            lastSpawnTime = Time.time + interval;
        }
        
        private GameObject ChooseSpawnable()
        {
            if (totalWeight <= 0)
            {
                return null;
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

            return data.Spawnables.Count > 0 ? data.Spawnables[^1].Prefab : null;
        }

        private Transform ChooseSpawnPoint()
        {
            if (spawnPoints != null && spawnPoints.Count != 0)
            {
                int index = Random.Range(0, spawnPoints.Count);
                return spawnPoints[index];
            }

            return null;
        }
    }
}