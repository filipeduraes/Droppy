using System;
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

        private IFlowController controller;
        private IGridContainer container;
        private IEndScreenViewModel endScreenViewModel;
        
        private void Awake()
        {
            if (flowController != null)
            {
                SetFlowController(flowController);
            }

            if (gridContainer != null)
            {
                SetGridContainer(gridContainer);
            }

            if (viewModel != null)
            {
                SetEndScreenViewModel(viewModel);
            }
        }

        private void Start()
        {
            controller.OnFlowFinished += CalculateResultsAndFinish;
        }

        private void OnDestroy()
        {
            controller.OnFlowFinished -= CalculateResultsAndFinish;
        }
        
        public void SetFlowController(IFlowController newFlowController)
        {
            controller = newFlowController;
        }

        public void SetGridContainer(IGridContainer newGridContainer)
        {
            container = newGridContainer;
        }

        public void SetEndScreenViewModel(IEndScreenViewModel newEndScreenViewModel)
        {
            endScreenViewModel = newEndScreenViewModel;
        }
        
        private void CalculateResultsAndFinish()
        {
            if (controller.Leaked)
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
            HashSet<Vector2Int> visitedPorts = controller.VisitedPorts;
            GridData grid = container.Grid;

            bool allExitsWereVisited = grid.Exits.TrueForAll(PortWasVisited);
            int starCount = 1;

            if (allExitsWereVisited)
            {
                starCount++;
            }

            bool allLockedPiecesWereVisited = container.Pieces.Where(piece => piece.Value.IsLocked)
                .All(piece => piece.Value.IsFull);

            if (allLockedPiecesWereVisited)
            {
                starCount++;
            }
            
            endScreenViewModel.RequestVictory(endScreenQuotes, starCount);
            return;

            bool PortWasVisited(GridPort port)
            {
                return visitedPorts.Contains(port.GetPortIndex(grid.Size));
            }
        }

        private void GameOverWithRetry()
        {
            endScreenViewModel.RequestDefeat(endScreenQuotes);
        }
    }
}