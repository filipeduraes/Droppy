using UnityEngine;

namespace Droppy.PieceMinigame
{
    public class GridContainer : MonoBehaviour
    {
        [SerializeField] private GridData grid;
        [SerializeField, Min(0)] private float cellSize = 1.0f;
        [SerializeField] private Vector2 offset;
        [SerializeField] private bool applyCenterOffset;

        public GridData Grid => grid;
        public float CellSize => cellSize;
        
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