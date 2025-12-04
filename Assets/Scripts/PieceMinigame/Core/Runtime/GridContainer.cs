using System.Collections.Generic;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
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
        [SerializeField] private SpriteRenderer entryPrefab;
        [SerializeField] private SpriteRenderer exitPrefab;

        public Dictionary<Vector2Int, Piece> Pieces { get; } = new();
        public Dictionary<Vector2Int, SpriteRenderer> Entries { get; } = new();
        public Dictionary<Vector2Int, SpriteRenderer> Exits { get; } = new();
        public CellData[,] RuntimeGrid { get; private set; }
        public GridData Grid => grid;
        public float CellSize => cellSize;


        private void Awake()
        {
            SpawnGrid();
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
        
        private void SpawnGrid()
        {
            RuntimeGrid = Grid.ConvertRowsToGrid();
            
            for (int y = 0; y < Grid.Size.y; y++)
            {
                for (int x = 0; x < Grid.Size.x; x++)
                {
                    CellData cell = RuntimeGrid[x, y];
                    
                    if (cell.Piece != null)
                    {
                        Vector3 position = GetCellCenterPosition(x, y);
                        Piece pieceInstance = Instantiate(piecePrefab, position, Quaternion.identity, transform);
                        pieceInstance.Populate(cell, new Vector2Int(x, y));
                        
                        Pieces[new Vector2Int(x, y)] = pieceInstance;
                    }
                }
            }

            SpawnPorts(grid.Entries, entryPrefab, Entries);
            SpawnPorts(grid.Exits, exitPrefab, Exits);
        }

        private void SpawnPorts(IEnumerable<GridPort> ports, SpriteRenderer portPrefab, Dictionary<Vector2Int, SpriteRenderer> portsMap)
        {
            foreach (GridPort entry in ports)
            {
                Vector2Int entryPortIndex = entry.GetPortIndex(Grid.Size);
                Vector2Int adjacentPortIndex = entry.GetAdjacentIndex(grid.Size);
                
                Vector2 direction = adjacentPortIndex - entryPortIndex;
                direction = direction.normalized;
                
                Quaternion rotation = Quaternion.FromToRotation(Vector2.up, direction);
                Vector3 entryPosition = GetCellCenterPosition(entryPortIndex.x, entryPortIndex.y);

                SpriteRenderer portSpriteRenderer = Instantiate(portPrefab, entryPosition, rotation, transform);
                portsMap[entryPortIndex] = portSpriteRenderer;
            }
        }
    }
}