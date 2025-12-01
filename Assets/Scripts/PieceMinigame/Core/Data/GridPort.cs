using System;
using UnityEngine;

namespace Droppy.PieceMinigame.Data
{
    [Serializable]
    public class GridPort
    {
        [SerializeField] private PieceDirection direction = PieceDirection.Left;
        [SerializeField] private int offset = 0;

        public PieceDirection Direction
        {
            get => direction;
            set => direction = value;
        }

        public int Offset
        {
            get => offset;
            set => offset = value;
        }

        public Vector2Int GetAdjacentIndex(Vector2Int gridSize)
        {
            return direction switch
            {
                PieceDirection.Right => new Vector2Int(gridSize.x - 1, offset),
                PieceDirection.Bottom => new Vector2Int(offset, 0),
                PieceDirection.Left => new Vector2Int(0, offset),
                PieceDirection.Top => new Vector2Int(offset, gridSize.y - 1),
                _ => -Vector2Int.one
            };
        }
        
        public Vector2Int GetPortIndex(Vector2Int gridSize)
        {
            return direction switch
            {
                PieceDirection.Right => new Vector2Int(gridSize.x, offset),
                PieceDirection.Bottom => new Vector2Int(offset, -1),
                PieceDirection.Left => new Vector2Int(-1, offset),
                PieceDirection.Top => new Vector2Int(offset, gridSize.y),
                _ => -Vector2Int.one
            };
        }
    }
}