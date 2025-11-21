using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Droppy.PieceMinigame.Editor
{
    [CustomEditor(typeof(GridData))]
    public class GridDataEditor : UnityEditor.Editor
    {
        private Vector2 scrollPosition = Vector2.zero;
        
        private static readonly Vector2Int DefaultGridSize = new(4, 4);
        private const float CellPaddingSize = 2;
        private const string SizeFieldLabel = "Size";

        public override void OnInspectorGUI()
        {
            if (target is not GridData gridData)
            {
                return;
            }

            DrawSizeField(gridData);

            EditorGUILayout.Space(10);

            DrawGrid(gridData);
            
            EditorGUILayout.Space(20);

            EditorGUILayout.BeginVertical();
            {
                DrawPortsList(gridData, gridData.Entries, "Entry Ports");
                EditorGUILayout.Space(10);
                DrawPortsList(gridData, gridData.Exits, "Exit Ports");
                
            }
            EditorGUILayout.EndVertical();
        }

        private static void DrawPortsList(GridData gridData, List<GridPort> ports, string label)
        {
            EditorGUILayout.LabelField(label);

            foreach (GridPort port in ports)
            {
                DrawGridPortField(gridData, port);
                EditorGUILayout.Space(CellPaddingSize);
            }
            
            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("+"))
                {
                    ports.Add(new GridPort());
                    EditorUtility.SetDirty(gridData);
                }

                if (ports.Count > 0 && GUILayout.Button("-"))
                {
                    ports.RemoveAt(gridData.Entries.Count - 1);
                    EditorUtility.SetDirty(gridData);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawGridPortField(GridData gridData, GridPort port)
        {
            EditorGUILayout.BeginHorizontal();
            {
                bool isHorizontalDirection = (port.Direction & (PieceDirection.Top | PieceDirection.Bottom)) != 0;
                PieceDirection newDirection = (PieceDirection) EditorGUILayout.EnumPopup(port.Direction);
                        
                if (newDirection == PieceDirection.None)
                {
                    newDirection = port.Direction;
                }
                        
                int newOffset = EditorGUILayout.IntSlider(port.Offset, 0, (isHorizontalDirection ? gridData.Size.x : gridData.Size.y) - 1);

                bool isDirty = newDirection != port.Direction || newOffset != port.Offset;

                port.Direction = newDirection;
                port.Offset = newOffset;

                if (port.Direction == PieceDirection.None)
                {
                    port.Direction = PieceDirection.Left;
                }

                if (isDirty)
                {
                    EditorUtility.SetDirty(gridData);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSizeField(GridData gridData)
        {
            Vector2Int newGridSize = EditorGUILayout.Vector2IntField(SizeFieldLabel, gridData.Size);

            if (newGridSize == Vector2Int.zero && gridData.Size == Vector2Int.zero)
            {
                gridData.SetGridSize(DefaultGridSize); //Default value
            }

            if (newGridSize != gridData.Size)
            {
                gridData.SetGridSize(newGridSize);
                EditorUtility.SetDirty(gridData);
            }
        }

        private void DrawGrid(GridData gridData)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                EditorGUILayout.BeginVertical();
                {
                    for (int y = gridData.Size.y - 1; y >= 0; y--)
                    {
                        EditorGUILayout.Space(CellPaddingSize);
                        DrawRow(gridData, y);
                        EditorGUILayout.Space(CellPaddingSize);
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }

        private static void DrawRow(GridData gridData, int y)
        {
            EditorGUILayout.BeginHorizontal();
            {
                for (int x = 0; x < gridData.Size.x; x++)
                {
                    if (y >= gridData.Grid.Count || x >= gridData.Grid[y].RowCells.Count)
                    {
                        continue;
                    }

                    DrawCell(gridData, x, y);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private static void DrawCell(GridData gridData, int x, int y)
        {
            Rect rect = EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.Space(CellPaddingSize);

                DrawCellBackground(x, y, rect);
                DrawCellContents(gridData, x, y);

                EditorGUILayout.Space(CellPaddingSize);
            }
            EditorGUILayout.EndVertical();
        }

        private static void DrawCellBackground(int x, int y, Rect rect)
        {
            bool shouldDisplayGrayBackground = x % 2 == (y % 2 == 0 ? 0 : 1);

            if (shouldDisplayGrayBackground)
            {
                EditorGUI.DrawRect(rect, Color.gray);
            }
        }

        private static void DrawCellContents(GridData gridData, int x, int y)
        {
            CellData cellData = gridData.Grid[y].RowCells[x];
            PieceData selectedPiece = (PieceData) EditorGUILayout.ObjectField(cellData.Piece, typeof(PieceData), false);
                    
            if (selectedPiece != cellData.Piece)
            {
                cellData.Piece = selectedPiece;
                EditorUtility.SetDirty(gridData);
            }
                    
            EditorGUILayout.Space(CellPaddingSize);
                    
            DrawRotationSteps(gridData, cellData);
        }

        private static void DrawRotationSteps(GridData gridData, CellData cellData)
        {
            EditorGUILayout.BeginHorizontal();
            {
                for (int i = 0; i < 4; i++)
                {
                    bool isSelected = cellData.RotationSteps == i;

                    EditorGUI.BeginDisabledGroup(isSelected);
                    {
                        if (GUILayout.Button(i.ToString()) && !isSelected)
                        {
                            cellData.RotationSteps = i;
                            EditorUtility.SetDirty(gridData);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
