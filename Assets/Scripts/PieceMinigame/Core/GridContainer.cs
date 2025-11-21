using System;
using UnityEngine;

namespace Droppy.PieceMinigame
{
    public class GridContainer : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private GridData grid;
        [SerializeField, Min(0)] private float cellSize = 1.0f;
        [SerializeField] private Vector2 offset;
        [SerializeField] private bool applyCenterOffset;

        [Header("Prefabs")] 
        [SerializeField] private Piece piecePrefab;

        public GridData Grid => grid;
        public float CellSize => cellSize;
        
        private CellData[,] runtimeGrid;

        private void Awake()
        {
            SpawnGrid();
        }

        private void SpawnGrid()
        {
            runtimeGrid = Grid.ConvertRowsToGrid();
            
            for (int y = 0; y < Grid.Size.y; y++)
            {
                for (int x = 0; x < Grid.Size.x; x++)
                {
                    CellData cell = runtimeGrid[x, y];
                    
                    if (cell.Piece != null)
                    {
                        Vector3 position = GetCellCenterPosition(x, y);
                        Piece pieceInstance = Instantiate(piecePrefab, position, Quaternion.identity, transform);
                        pieceInstance.Populate(cell, new Vector2Int(x, y));
                    }
                }
            }
        }

        public Vector3 GetPortBorderPosition(GridPort port)
        {
            return port.Direction switch
            {
                PieceDirection.Right => GetBorderPosition(Grid.Size.x - 1, port.Offset, port.Direction),
                PieceDirection.Bottom => GetBorderPosition(port.Offset, 0, port.Direction),
                PieceDirection.Left => GetBorderPosition(0, port.Offset, port.Direction),
                PieceDirection.Top => GetBorderPosition(port.Offset, Grid.Size.y - 1, port.Direction),
                _ =>  Vector3.zero
            };
        }
        
        public Vector3 GetBorderPosition(int x, int y, PieceDirection direction)
        {
            Vector3 centerPosition = GetCellCenterPosition(x, y);
            
            foreach (Vector3 directionOffset in direction.ToVectors())
            {
                centerPosition += directionOffset * CellSize * 0.5f;
            }

            return centerPosition;
        }
        
        public Vector3 GetCellCenterPosition(int x, int y)
        {
            return GetCellPosition(x, y) + (Vector3)(Vector2.one * CellSize * 0.5f);
        }
        
        public Vector3 GetCellPosition(int x, int y)
        {
            Vector3 cellPosition = transform.position + (Vector3) (offset + new Vector2(x, y) * CellSize);

            if (applyCenterOffset)
            {
                Vector3 centerOffset = (Vector2) grid.Size * CellSize * 0.5f;
                cellPosition -= centerOffset;
            }
            
            return cellPosition;
        }
    }
}