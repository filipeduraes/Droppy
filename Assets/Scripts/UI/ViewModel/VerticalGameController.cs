using UnityEngine;
using Droppy.UI.ViewModel;

namespace Droppy.UI
{
    public abstract class VerticalGameController : MonoBehaviour
    {
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private EndScreenResultQuotes endScreenQuotes;

        public void HandleLevelFinished(bool isVictory)
        {
            if (isVictory)
            {
                GameOverWithVictory();
            }
            else
            {
                GameOverWithDefeat();
            }
        }

        private void GameOverWithVictory()
        {
            int starCount = 1;
            viewModel.RequestVictory(endScreenQuotes, starCount);
        }

        private void GameOverWithDefeat()
        {
            viewModel.RequestDefeat(endScreenQuotes);
        }
    }
}