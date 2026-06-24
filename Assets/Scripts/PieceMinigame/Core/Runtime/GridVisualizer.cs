using System;
using System.Collections.Generic;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private GridContainer container;
        [SerializeField] private FlowController flowController;
        [SerializeField] private float portSize = 0.15f;
        [SerializeField] private float visitedPortSize = 0.35f;
        [SerializeField] private float fontSize = 15f;

        private float CellSize => gridContainer != null ? gridContainer.CellSize : 1.0f;
        private GridData Grid => gridContainer != null ? gridContainer.Grid : null;

        private IFlowController controller;
        private IGridContainer gridContainer;

        private void Awake()
        {
            SetFlowController(flowController);
            SetGridContainer(container);
        }

        private void OnDrawGizmos()
        {
            if (gridContainer == null || Grid == null)
            {
                return;
            }

            DrawGridLinesGizmos();
            DrawGridPiecesGizmos();

            Gizmos.color = Color.blue;
            DrawPorts(Grid.Entries);
            
            Gizmos.color = Color.red;
            DrawPorts(Grid.Exits);
        }

        public void SetFlowController(IFlowController newFlowController)
        {
            controller = newFlowController;
        }

        public void SetGridContainer(IGridContainer newGridContainer)
        {
            gridContainer = newGridContainer;
        }

        private void DrawPorts(List<GridPort> ports)
        {
            foreach (GridPort port in ports)
            {
                float radius = portSize;
                
                if (controller != null && controller.VisitedPorts != null && controller.VisitedPorts.Contains(port.GetPortIndex(Grid.Size)))
                {
                    radius = visitedPortSize;
                }
                
                Gizmos.DrawSphere(gridContainer.GetPortBorderPosition(port), radius);
            }
        }

        private void DrawGridLinesGizmos()
        {
            Gizmos.color = Color.green;
            
            for (int y = 0; y <= Grid.Size.y; y++)
            {
                if (y != Grid.Size.y)
                {
                    DrawLabelGizmos(y.ToString(), gridContainer.GetCellCenterPosition(-1, y), Color.green, Vector2.zero, fontSize);
                }
                
                Gizmos.DrawLine(gridContainer.GetCellPosition(0, y), gridContainer.GetCellPosition(Grid.Size.x, y));
            }
            
            for (int x = 0; x <= Grid.Size.x; x++)
            {
                if (x != Grid.Size.x)
                {
                    DrawLabelGizmos(x.ToString(), gridContainer.GetCellCenterPosition(x, -1), Color.green, Vector2.zero, fontSize);
                }
                
                Gizmos.DrawLine(gridContainer.GetCellPosition(x, 0), gridContainer.GetCellPosition(x, Grid.Size.y));
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
                        
                        Vector3 centerPosition = gridContainer.GetCellCenterPosition(x, y);
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