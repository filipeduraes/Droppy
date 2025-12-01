using System;
using UnityEngine;

namespace Droppy.UI.ViewModel
{
    [CreateAssetMenu(fileName = "End Screen View Model", menuName = "Droppy/View Model/End Screen", order = 0)]
    public class EndScreenViewModel : ScriptableObject
    {
        [SerializeField] private EndScreenResultQuotes quotes;
        [SerializeField] private int starCount = 3;
        [SerializeField] private bool isVictory = true;
            
        public event Action OnShowEndScreenRequested = delegate { };
        public event Action OnRetryRequested = delegate { };
        public event Action OnNextLevelRequested = delegate { };

        public int StarCount => starCount;

        public bool IsVictory => isVictory;

        public string GetRandomQuote()
        {
            if (quotes == null)
            {
                return string.Empty;
            } 

            return IsVictory
                    ? quotes.GetRandomWinQuoteFromStarCount(StarCount) 
                    : quotes.GetRandomDefeatQuote();
        }

        public void RequestVictory(EndScreenResultQuotes newQuotes, int newStarCount)
        {
            quotes = newQuotes;
            starCount = newStarCount;
            isVictory = true;
            RequestShowEndScreen();
        }
        
        public void RequestDefeat(EndScreenResultQuotes newQuotes)
        {
            quotes = newQuotes;
            starCount = 0;
            isVictory = false;
            RequestShowEndScreen();
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