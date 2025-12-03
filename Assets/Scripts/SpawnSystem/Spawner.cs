using UnityEngine;
using System.Collections.Generic;
using Droppy.Shared;
using IdeaToGame.ObjectPooling;


namespace Droppy.SpawnSystem
{
    public class Spawner : MonoBehaviour
    {
        [SerializeField] private SpawnerData data;
        [SerializeField] private List<Transform> spawnPoints = new();
        [SerializeField] private bool destroyAllOnDisable = true;
        [SerializeField] private bool startOnAwake = false;
        [SerializeField] private bool preventSpawnOnSamePoint = false;

        private readonly Dictionary<Transform, Transform> occupiedSlots = new();
        private List<Transform> freeSlots;

        private bool isRunning = false;
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

            if (preventSpawnOnSamePoint)
            {
                freeSlots.AddRange(spawnPoints);
            }

            if (startOnAwake)
            {
                StartSpawner();
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
            if (isRunning && Time.time > lastSpawnTime)
            {
                SpawnObject();
            }
        }
        
        public void StartSpawner()
        {
            isRunning = true;
        }

        public void StopSpawner()
        {
            isRunning = false;
        }

        private void SpawnObject()
        {
            if (preventSpawnOnSamePoint)
            {
                UpdateOccupiedSlots();

                if (occupiedSlots.Count == spawnPoints.Count)
                {
                    return;
                }
            }
            
            GameObject prefabToSpawn = ChooseSpawnable();
            Transform spawnPoint = ChooseSpawnPoint();
            Transform spawnedTransform = ObjectPool.GetFromPool(prefabToSpawn.transform);

            spawnedTransform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
            
            float interval = Random.Range(data.MinSpawnInterval, data.MaxSpawnInterval);
            lastSpawnTime = Time.time + interval;

            if (preventSpawnOnSamePoint)
            {
                occupiedSlots[spawnPoint] = spawnedTransform;
                freeSlots.Remove(spawnPoint);
            }
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
                if (preventSpawnOnSamePoint)
                {
                    return freeSlots.GetRandomElement();
                }
                
                return spawnPoints.GetRandomElement();
            }

            return null;
        }

        private void UpdateOccupiedSlots()
        {
            HashSet<Transform> slotsToRemove = new();
            
            foreach ((Transform slot, Transform spawnedObject) in occupiedSlots)
            {
                if (!spawnedObject.gameObject.activeSelf)
                {
                    slotsToRemove.Add(slot);
                }
            }

            foreach (Transform slot in slotsToRemove)
            {
                occupiedSlots.Remove(slot);
                freeSlots.Add(slot);
            }
        }
    }
}