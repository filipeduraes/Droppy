using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Droppy.UI.ViewModel
{
    [Serializable]
    public class QuoteContainer
    {
        [SerializeField] private List<string> quotes;

        public string GetRandomQuote()
        {
            int randomQuoteIndex = Random.Range(0, quotes.Count);
            return quotes[randomQuoteIndex];
        }
    }
    
    [CreateAssetMenu(fileName = "Result Quotes", menuName = "Droppy/UI/End Screen Quotes", order = 0)]
    public class EndScreenResultQuotes : ScriptableObject
    {
        [SerializeField] private List<QuoteContainer> victoryStarQuotes;
        [SerializeField] private QuoteContainer defeatQuotes;

        public string GetRandomWinQuoteFromStarCount(int starCount)
        {
            int index = Mathf.Clamp(starCount - 1, 0, victoryStarQuotes.Count - 1);
            return victoryStarQuotes[index].GetRandomQuote();
        }

        public string GetRandomDefeatQuote()
        {
            return defeatQuotes.GetRandomQuote();
        }
    }
}