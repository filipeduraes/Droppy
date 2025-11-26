using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public class FlowController : MonoBehaviour
    {
        [SerializeField] private GridContainer gridContainer;
        private Queue<Vector2Int> searchHeads;
        private HashSet<Vector2Int> visited;

        private void Start()
        {
            StartCoroutine(FlowThroughGrid());
        }

        private IEnumerator FlowThroughGrid()
        {
            List<GridPort> entries = gridContainer.Grid.Entries;
            searchHeads = new Queue<Vector2Int>();
            visited = new HashSet<Vector2Int>();

            foreach (GridPort entry in entries)
            {
                Vector2Int index = entry.GetAdjacentIndex(gridContainer.Grid.Size);

                if(!IsEmptyOrInvalidCell(index))
                {
                    searchHeads.Enqueue(index);
                    yield return new WaitForSeconds(0.5f);
                }
            }

            while (searchHeads.Count > 0)
            {
                Vector2Int head = searchHeads.Dequeue();
                visited.Add(head);

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        Vector2Int adjacentIndex = head + new Vector2Int(i, j);

                        if (i == j || adjacentIndex == head || IsEmptyOrInvalidCell(adjacentIndex))
                        {
                            continue;
                        }

                        Piece headPiece = gridContainer.Pieces[head];
                        Piece adjacentPiece = gridContainer.Pieces[adjacentIndex];

                        Vector2Int directionToAdjacent = adjacentPiece.Index - headPiece.Index;

                        bool isConnected = (directionToAdjacent.ToPieceDirection().Opposite() & adjacentPiece.Direction) != 0;
                        
                        if (isConnected)
                        {
                            searchHeads.Enqueue(adjacentIndex);
                        }
                        
                        yield return new WaitForSeconds(0.5f);
                    }
                }

                yield return new WaitForSeconds(0.5f);
            }
            
            yield return null;
        }

        private bool IsEmptyOrInvalidCell(Vector2Int adjacentIndex)
        {
            return !gridContainer.Grid.IsValidGridIndex(adjacentIndex) 
                   || !gridContainer.Pieces.ContainsKey(adjacentIndex)
                   || visited.Contains(adjacentIndex);
        }

        private void OnDrawGizmos()
        {
            if (visited == null || searchHeads == null)
            {
                return;
            }
            
            Gizmos.color = Color.blue;
            
            foreach (Vector2Int visitedIndex in visited)
            {
                if (!searchHeads.Contains(visitedIndex))
                {
                    Gizmos.DrawSphere(gridContainer.GetCellCenterPosition(visitedIndex.x, visitedIndex.y), 0.1f);
                }
            }
            
            Gizmos.color = Color.green;
            
            foreach (Vector2Int head in searchHeads)
            {
                Gizmos.DrawSphere(gridContainer.GetCellCenterPosition(head.x, head.y), 0.15f);
            }
        }
    }
}