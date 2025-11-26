using System;
using System.Collections.Generic;
using UnityEngine;

namespace Droppy.ServiceLocatorSystem
{
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, Component> Services = new();

        public static void Register(Component service)
        {
            Services[service.GetType()] = service;
        }
        
        public static bool TryGetService<T>(out T service) where T : Component
        {
            service = null;
            
            if (Services.ContainsKey(typeof(T)) && Services[typeof(T)] is T foundService)
            {
                service = foundService;
            }
            
            return service != null;
        }
    }
}