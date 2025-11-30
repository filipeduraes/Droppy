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
            flowController.OnFlowLeaked += ShowFlowLeak;
        }

        private void OnDisable()
        {
            flowController.OnFlowUpdate -= UpdateView;
            flowController.OnFlowLeaked -= ShowFlowLeak;
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
        
        private void ShowFlowLeak(FlowLeakInformation leak)
        {
            flowController.Stop();
            Debug.Log($"Leaked: {leak.headIndex}, {leak.adjacentIndex}");
        }
    }
}