using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
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

            Gizmos.color = Color.magenta;
            
            foreach (GridPort entryPort in Grid.Entries)
            {
                Gizmos.DrawSphere(container.GetPortBorderPosition(entryPort), 0.2f);
            }
            
            Gizmos.color = Color.red;
            
            foreach (GridPort exitPort in Grid.Exits)
            {
                Gizmos.DrawSphere(container.GetPortBorderPosition(exitPort), 0.2f);
            }
        }

        private void DrawGridLinesGizmos()
        {
            Gizmos.color = Color.green;
            
            for (int y = 0; y <= Grid.Size.y; y++)
            {
                if (y != Grid.Size.y)
                {
                    DrawLabelGizmos(y.ToString(), container.GetCellCenterPosition(-1, y), Color.green, Vector2.zero);
                }
                
                Gizmos.DrawLine(container.GetCellPosition(0, y), container.GetCellPosition(Grid.Size.x, y));
            }
            
            for (int x = 0; x <= Grid.Size.x; x++)
            {
                if (x != Grid.Size.y)
                {
                    DrawLabelGizmos(x.ToString(), container.GetCellCenterPosition(x, -1), Color.green, Vector2.zero);
                }
                
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
        
        private static void DrawLabelGizmos(string text, Vector3 worldPosition, Color textColor, Vector2 anchor, float textSize = 15f)
        {
        #if UNITY_EDITOR
            UnityEditor.SceneView view = UnityEditor.SceneView.currentDrawingSceneView;
            
            if (!view)
            {
                return;
            }
            
            Vector3 screenPosition = view.camera.WorldToScreenPoint(worldPosition);

            if (screenPosition.y < 0 || screenPosition.y > view.camera.pixelHeight || screenPosition.x < 0 || screenPosition.x > view.camera.pixelWidth || screenPosition.z < 0)
            {
                return;
            }
            
            float pixelRatio = UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.right).x - UnityEditor.HandleUtility.GUIPointToScreenPixelCoordinate(Vector2.zero).x;
            UnityEditor.Handles.BeginGUI();
            
            GUIStyle style = new(GUI.skin.label)
            {
                fontSize = (int)textSize,
                normal = new GUIStyleState { textColor = textColor }
            };
            
            Vector2 size = style.CalcSize(new GUIContent(text)) * pixelRatio;
            Vector2 alignedPosition = ((Vector2)screenPosition + size * ((anchor + Vector2.left + Vector2.up) / 2f)) * (Vector2.right + Vector2.down) + Vector2.up * view.camera.pixelHeight;
            GUI.Label(new Rect(alignedPosition / pixelRatio, size / pixelRatio), text, style);
            UnityEditor.Handles.EndGUI();
        #endif
        }
    }
}