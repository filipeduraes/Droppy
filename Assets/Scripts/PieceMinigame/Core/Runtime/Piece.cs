using Droppy.InteractionSystem;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public class Piece : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer visual;

        public PieceDirection Direction { get; private set; }
        public bool IsFull { get; private set; }
        
        public Vector2Int Index { get; private set; }
        private PieceData pieceData;

        public void Populate(CellData cellData, Vector2Int pieceIndex)
        {
            pieceData = cellData.Piece;
            visual.sprite = pieceData.Sprite;
            Index = pieceIndex;

            Direction = pieceData.DefaultDirections.RotateClockwise(cellData.RotationSteps);
            transform.Rotate(-Vector3.forward, cellData.RotationSteps * 90.0f);
        }

        public void Fill()
        {
            visual.sprite = pieceData.FullSprite;
            IsFull = true;
        }
        
        public void Interact(GameObject agent)
        {
            if (!IsFull)
            {
                RotateClockwise();
            }
        }

        private void RotateClockwise()
        {
            Direction = Direction.RotateClockwise();
            transform.Rotate(-Vector3.forward, 90.0f);
        }
    }
}