using System;
using System.Collections.Generic;
using System.Linq;
using Droppy.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame
{
    [CreateAssetMenu(menuName = "Droppy/Piece Minigame/Grid", fileName = "GridData")]
    public class GridData : ScriptableObject
    {
        [SerializeField] private Vector2Int size = new Vector2Int(4, 4);
        [SerializeField] private List<GridRow> grid;
        [SerializeField] private List<Vector2Int> entries;
        [SerializeField] private List<Vector2Int> exits;

        public List<GridRow> Grid
        {
            get => grid;
            set => grid = value;
        }

        public Vector2Int Size => size;

        public CellData[,] ConvertRowsToGrid()
        {
            CellData[,] convertedGrid = new CellData[Size.x, Size.y];
            
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    if (y >= grid.Count || x >= grid[y].RowCells.Count)
                    {
                        continue;
                    }
                    
                    convertedGrid[x, y] = grid[y].RowCells[x];
                }
            }

            return convertedGrid;
        }

        public void SetGridSize(Vector2Int newGridSize)
        {
            bool isSameSizeAsCurrent = newGridSize == size;
            
            if ((isSameSizeAsCurrent && CurrentGridSizeIsValid()) || newGridSize.x < 0 || newGridSize.y < 0)
            {
                return;
            }

            grid ??= new List<GridRow>();
            grid.Resize(newGridSize.y, () => new GridRow(newGridSize.x));

            foreach (GridRow row in grid)
            {
                row.RowCells.Resize(newGridSize.x, () => new CellData());
            }

            size = newGridSize;
        }

        private bool CurrentGridSizeIsValid()
        {
            return grid.Count == size.y && grid.All(row => row.RowCells.Count == size.x);
        }
    }

    [Serializable]
    public class GridRow
    {
        [SerializeField] private List<CellData> rowCells;

        public List<CellData> RowCells => rowCells;

        public GridRow(int rowSize)
        {
            rowCells = new List<CellData>();
        }
    }
}