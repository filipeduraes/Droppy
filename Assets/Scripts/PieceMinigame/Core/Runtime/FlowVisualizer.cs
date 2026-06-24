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
        
        private IFlowController controller;
        private IGridContainer container;
        
        
        private void Awake()
        {
            SetFlowController(flowController);
            SetGridContainer(gridContainer);
        }
        
        private void OnEnable()
        {
            controller.OnFlowStarted += PlayFlowStartAnimation;
            controller.OnFlowUpdate += UpdateView;
            controller.OnFlowLeaked += ShowFlowLeak;
        }

        private void OnDisable()
        {
            controller.OnFlowStarted -= PlayFlowStartAnimation;
            controller.OnFlowUpdate -= UpdateView;
            controller.OnFlowLeaked -= ShowFlowLeak;
        }
        
        public void SetFlowController(IFlowController newFlowController)
        {
            controller = newFlowController;
        }

        public void SetGridContainer(IGridContainer newGridContainer)
        {
            container = newGridContainer;
        }

        private void PlayFlowStartAnimation()
        {
            foreach (SpriteRenderer entry in container.Entries.Values)
            {
                entry.sprite = openedEntrySprite;
            }
        }

        private void UpdateView()
        {
            foreach (Vector2Int visitedIndex in controller.Visited)
            {
                if(container.Pieces.TryGetValue(visitedIndex, out IPiece piece))
                {
                    piece.Fill();
                }
            }

            foreach ((Vector2Int index, SpriteRenderer exit) in container.Exits)
            {
                if (controller.VisitedPorts.Contains(index))
                {
                    exit.sprite = fullExitSprite;
                }
            }
        }
        
        private void ShowFlowLeak(FlowInformation leak)
        {
            controller.Stop();
            Debug.Log($"Leaked: {leak.headIndex}, {leak.adjacentIndex}");
        }
    }
}