using UnityEngine;

namespace Droppy.PieceMinigame
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GridContainer container;

        private float CellSize => container != null ? container.CellSize : 1.0f;
        private GridData Grid => container != null ? container.Grid : null;
        
        private void OnDrawGizmos()
        {
            if (container == null || Grid == null)
            {
                return;
            }

            DrawGridLinesGizmos();
            DrawGridPiecesGizmos();
        }

        private void DrawGridLinesGizmos()
        {
            Gizmos.color = Color.green;
            
            for (int y = 0; y <= Grid.Size.y; y++)
            {
                Gizmos.DrawLine(container.GetCellPosition(0, y), container.GetCellPosition(Grid.Size.x, y));
            }
            
            for (int x = 0; x <= Grid.Size.x; x++)
            {
                Gizmos.DrawLine(container.GetCellPosition(x, 0), container.GetCellPosition(x, Grid.Size.y));
            }
        }
        
        private void DrawGridPiecesGizmos()
        {
            CellData[,] cells = Grid.ConvertRowsToGrid();

            for (int y = 0; y < cells.GetLength(1); y++)
            {
                for (int x = 0; x < cells.GetLength(0); x++)
                {
                    CellData cell = cells[x, y];
                    
                    if (cell != null && cell.Piece != null)
                    {
                        Gizmos.color = cell.Piece.IsLocked ? Color.red : Color.blue;
                        PieceDirection directions = cell.Piece.DefaultDirections.RotateClockwise(cell.RotationSteps);
                        
                        Vector3 centerPosition = container.GetCellCenterPosition(x, y);
                        DrawPieceConnectionsGizmos(directions, centerPosition);
                    }
                }
            }
        }

        private void DrawPieceConnectionsGizmos(PieceDirection directions, Vector3 centerPosition)
        {
            foreach (Vector3 direction in directions.ToVectors())
            {
                Gizmos.DrawLine(centerPosition, centerPosition + direction * CellSize * 0.5f);
            }
        }
    }
}