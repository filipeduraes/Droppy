using System;
using UnityEngine;

namespace Droppy.SpawnSystem
{
    [Serializable]
    public class Spawnable 
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float weight = 1f;

        public GameObject Prefab => prefab;
        public float Weight => weight;
    }
}