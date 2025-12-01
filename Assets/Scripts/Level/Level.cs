using System;
using UnityEngine;

namespace Droppy.Level
{
    public class Level : MonoBehaviour
    {
        public event Action OnStarted = delegate { };

        public void StartLevel()
        {
            OnStarted();
        }
    }
}