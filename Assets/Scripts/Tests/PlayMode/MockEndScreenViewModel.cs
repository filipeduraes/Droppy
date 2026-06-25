using Droppy.UI.ViewModel;

namespace Droppy.Tests.PlayMode
{
    public class MockEndScreenViewModel : IEndScreenViewModel
    {
        public bool VictoryRequested { get; private set; }
        public bool DefeatRequested { get; private set; }
        public int StarsCount { get; private set; }

        public void RequestVictory(EndScreenResultQuotes resultQuotes, int starsResultCount)
        {
            VictoryRequested = true;
            StarsCount = starsResultCount;
        }

        public void RequestDefeat(EndScreenResultQuotes resultQuotes)
        {
            DefeatRequested = true;
        }
    }
}