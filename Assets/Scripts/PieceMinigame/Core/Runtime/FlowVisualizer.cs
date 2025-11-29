using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public class FlowVisualizer : MonoBehaviour
    {
        [SerializeField] private FlowController flowController;
        [SerializeField] private GridContainer gridContainer;
        
        private void OnEnable()
        {
            flowController.OnFlowUpdate += UpdateView;
        }

        private void OnDisable()
        {
            flowController.OnFlowUpdate -= UpdateView;
        }
        
        private void UpdateView()
        {
            foreach (Vector2Int visitedIndex in flowController.Visited)
            {
                if(gridContainer.Pieces.TryGetValue(visitedIndex, out Piece piece))
                {
                    piece.Fill();
                }
            }
        }
    }
}