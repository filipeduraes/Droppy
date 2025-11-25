using UnityEngine;

namespace Droppy.ServiceLocatorSystem
{
    public class ServiceRegister : MonoBehaviour
    {
        [SerializeField] private Component service;

        private void Awake()
        {
            ServiceLocator.Register(service);
        }
    }
}