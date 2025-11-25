using System;
using UnityEngine;

namespace Droppy.Level
{
    public class Level : MonoBehaviour
    {
        public event Action OnStarted = delegate { };
        public event Action OnFinished = delegate { };

        public void StartLevel()
        {
            OnStarted();
        }
    }
}