using Droppy.InteractionSystem;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public class Piece : MonoBehaviour, IInteractable
    {
        [SerializeField] private SpriteRenderer visual;

        private Vector2Int index;
        private PieceDirection currentDirection;
        private PieceData pieceData;

        public void Populate(CellData cellData, Vector2Int pieceIndex)
        {
            pieceData = cellData.Piece;
            visual.sprite = pieceData.Sprite;
            index = pieceIndex;

            currentDirection = pieceData.DefaultDirections.RotateClockwise(cellData.RotationSteps);
            transform.Rotate(-Vector3.forward, cellData.RotationSteps * 90.0f);
        }
        
        public void Interact(GameObject agent)
        {
            RotateClockwise();
        }

        private void RotateClockwise()
        {
            currentDirection = currentDirection.RotateClockwise();
            transform.Rotate(-Vector3.forward, 90.0f);
        }
    }
}