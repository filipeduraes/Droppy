using UnityEngine;

namespace Droppy.PieceMinigame
{
    public class GridContainer : MonoBehaviour
    {
        [SerializeField] private GridData grid;
        [SerializeField, Min(0)] private float cellSize = 1.0f;

        private void OnDrawGizmos()
        {
            if (grid == null)
            {
                return;
            }

            DrawGridLinesGizmos();
            DrawGridPiecesGizmos();
        }

        private void DrawGridLinesGizmos()
        {
            Gizmos.color = Color.green;
            
            for (int y = 0; y <= grid.Size.y; y++)
            {
                Gizmos.DrawLine(GetCellPosition(0, y), GetCellPosition(grid.Size.x, y));
            }
            
            for (int x = 0; x <= grid.Size.x; x++)
            {
                Gizmos.DrawLine(GetCellPosition(x, 0), GetCellPosition(x, grid.Size.y));
            }
        }
        
        private void DrawGridPiecesGizmos()
        {
            CellData[,] cells = grid.ConvertRowsToGrid();

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    CellData cell = cells[x, y];
                    
                    if (cell != null && cell.Piece != null)
                    {
                        Gizmos.color = cell.Piece.IsLocked ? Color.red : Color.blue;
                        PieceDirection directions = cell.Piece.DefaultDirections.RotateClockwise(cell.RotationSteps);
                        
                        Vector3 centerPosition = GetCellCenterPosition(x, y);
                        DrawPieceConnectionsGizmos(directions, centerPosition);
                    }
                }
            }
        }

        private void DrawPieceConnectionsGizmos(PieceDirection directions, Vector3 centerPosition)
        {
            foreach (Vector3 direction in directions.ToVectors())
            {
                Gizmos.DrawLine(centerPosition, centerPosition + direction * cellSize * 0.5f);
            }
        }

        private Vector3 GetCellCenterPosition(int x, int y)
        {
            return GetCellPosition(x, y) + (Vector3)(Vector2.one * cellSize * 0.5f);
        }
        
        private Vector3 GetCellPosition(int x, int y)
        {
            return transform.position + new Vector3(x, y, 0) * cellSize;
        }
    }
}