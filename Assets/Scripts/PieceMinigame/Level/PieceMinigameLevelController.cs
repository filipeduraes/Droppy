using System.Collections.Generic;
using System.Linq;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Runtime;
using Droppy.UI.ViewModel;
using UnityEngine;

namespace Droppy.PieceMinigame.Level
{
    public class PieceMinigameLevelController : MonoBehaviour
    {
        [SerializeField] private FlowController flowController;
        [SerializeField] private GridContainer gridContainer;
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private EndScreenResultQuotes endScreenQuotes;

        private void OnEnable()
        {
            flowController.OnFlowFinished += CalculateResultsAndFinish;
        }

        private void OnDisable()
        {
            flowController.OnFlowFinished -= CalculateResultsAndFinish;
        }
        
        private void CalculateResultsAndFinish()
        {
            if (flowController.Leaked)
            {
                GameOverWithRetry();
            }
            else
            {
                GameOverWithVictory();
            }
        }

        private void GameOverWithVictory()
        {
            HashSet<Vector2Int> visitedPorts = flowController.VisitedPorts;
            GridData grid = gridContainer.Grid;

            bool allExitsWereVisited = grid.Exits.TrueForAll(PortWasVisited);
            int starCount = 1;

            if (allExitsWereVisited)
            {
                starCount++;
            }

            bool allLockedPiecesWereVisited = gridContainer.Pieces.Where(piece => piece.Value.IsLocked)
                .All(piece => piece.Value.IsFull);

            if (allLockedPiecesWereVisited)
            {
                starCount++;
            }
            
            viewModel.RequestVictory(endScreenQuotes, starCount);
            return;

            bool PortWasVisited(GridPort port)
            {
                return visitedPorts.Contains(port.GetPortIndex(grid.Size));
            }
        }

        private void GameOverWithRetry()
        {
            viewModel.RequestDefeat(endScreenQuotes);
        }
    }
}