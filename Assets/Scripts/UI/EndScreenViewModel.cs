using System;
using UnityEngine;

namespace Droppy.UI
{
    [CreateAssetMenu(fileName = "End Screen View Model", menuName = "Droppy/View Model/End Screen", order = 0)]
    public class EndScreenViewModel : ScriptableObject
    {
        [SerializeField] private string quote;
        [SerializeField] private int starCount = 3;
        [SerializeField] private bool isContinueEnabled = true;
            
        public event Action OnShowEndScreenRequested = delegate { };
        public event Action OnRetryRequested = delegate { };
        public event Action OnNextLevelRequested = delegate { };

        public string Quote
        {
            get => quote;
            set => quote = value;
        }

        public int StarCount
        {
            get => starCount;
            set => starCount = value;
        }

        public bool IsContinueEnabled
        {
            get => isContinueEnabled;
            set => isContinueEnabled = value;
        }

        [ContextMenu("Show End Screen")]
        public void RequestShowEndScreen()
        {
            OnShowEndScreenRequested();
        }
        
        [ContextMenu("Retry")]
        public void RequestRetry()
        {
            OnRetryRequested();
        }

        [ContextMenu("Next Level")]
        public void RequestNextLevel()
        {
            OnNextLevelRequested();
        }
    }
}