using System;
using UnityEngine;

namespace Droppy.UI.ViewModel
{
    [CreateAssetMenu(fileName = "Pause Screen View Model", menuName = "Droppy/View Model/Pause Screen", order = 0)]
    public class PauseScreenViewModel : ScriptableObject
    {
        public bool IsPaused { get; private set; } = false;
        public bool IsPauseEnabled { get; private set; } = false;
        
        public event Action<bool> OnPauseRequested = delegate { };
        public event Action OnMainMenuRequested = delegate { };
        public event Action OnExitRequested = delegate { };
        
        public void RequestPause()
        {
            if (!IsPauseEnabled)
            {
                return;
            }
            
            IsPaused = !IsPaused;
            OnPauseRequested(IsPaused);
        }

        public void RequestMainMenu()
        {
            OnMainMenuRequested();
        }

        public void RequestExit()
        {
            OnExitRequested();
        }
        
        public void SetIsPauseEnabled(bool isEnabled)
        {
            IsPauseEnabled = isEnabled;
        }
    }
}