using System.Collections.Generic;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public class FlowVisualizer : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private FlowController flowController;
        [SerializeField] private GridContainer gridContainer;

        [Header("Sprites")]
        [SerializeField] private Sprite openedEntrySprite;
        [SerializeField] private Sprite fullExitSprite;
        
        private void OnEnable()
        {
            flowController.OnFlowStarted += PlayFlowStartAnimation;
            flowController.OnFlowUpdate += UpdateView;
            flowController.OnFlowLeaked += ShowFlowLeak;
        }

        private void OnDisable()
        {
            flowController.OnFlowStarted -= PlayFlowStartAnimation;
            flowController.OnFlowUpdate -= UpdateView;
            flowController.OnFlowLeaked -= ShowFlowLeak;
        }

        private void PlayFlowStartAnimation()
        {
            foreach (SpriteRenderer entry in gridContainer.Entries.Values)
            {
                entry.sprite = openedEntrySprite;
            }
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

            foreach ((Vector2Int index, SpriteRenderer exit) in gridContainer.Exits)
            {
                if (flowController.VisitedPorts.Contains(index))
                {
                    exit.sprite = fullExitSprite;
                }
            }
        }
        
        private void ShowFlowLeak(FlowInformation leak)
        {
            flowController.Stop();
            Debug.Log($"Leaked: {leak.headIndex}, {leak.adjacentIndex}");
        }
    }
}