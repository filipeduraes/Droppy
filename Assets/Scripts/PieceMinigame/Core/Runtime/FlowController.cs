using System;
using System.Collections;
using System.Collections.Generic;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public struct FlowLeakInformation
    {
        public Vector2Int headIndex;
        public Vector2Int adjacentIndex;

        public FlowLeakInformation(Vector2Int headIndex, Vector2Int adjacentIndex)
        {
            this.headIndex = headIndex;
            this.adjacentIndex = adjacentIndex;
        }
    }
    
    public class FlowController : MonoBehaviour
    {
        [SerializeField] private GridContainer gridContainer;
        [SerializeField] private float secondsBeforeStartFlow = 5.0f;
        [SerializeField] private float secondsBetweenUpdates = 1.0f;

        public event Action OnFlowUpdate = delegate { };
        public event Action<FlowLeakInformation> OnFlowLeaked = delegate { };
        public IEnumerable<Vector2Int> Visited => visited;

        private Queue<Vector2Int> searchHeads;
        private HashSet<Vector2Int> visited;
        private Coroutine flowCoroutine;

        private void Start()
        {
            flowCoroutine = StartCoroutine(FlowThroughGrid());
        }
        
        public void Stop()
        {
            if (flowCoroutine != null)
            {
                StopCoroutine(flowCoroutine);
            }
        }

        private IEnumerator FlowThroughGrid()
        {
            searchHeads = new Queue<Vector2Int>();
            visited = new HashSet<Vector2Int>();

            yield return new WaitForSeconds(secondsBeforeStartFlow);
            FindHeadsFromPorts();
            OnFlowUpdate();
            
            yield return new WaitForSeconds(secondsBetweenUpdates);

            while (searchHeads.Count > 0)
            {
                List<Vector2Int> currentHeads = new();

                while (searchHeads.Count > 0)
                {
                    Vector2Int head = searchHeads.Dequeue();
                    visited.Add(head);
                    currentHeads.Add(head);
                }

                yield return new WaitForSeconds(secondsBetweenUpdates);

                foreach (Vector2Int head in currentHeads)
                {
                    SearchHead(head);
                }

                OnFlowUpdate();
                yield return new WaitForSeconds(secondsBetweenUpdates);
            }
            
            yield return null;
        }

        private void FindHeadsFromPorts()
        {
            foreach (GridPort entry in gridContainer.Grid.Entries)
            {
                Vector2Int portIndex = entry.GetPortIndex(gridContainer.Grid.Size);
                Vector2Int adjacentIndex = entry.GetAdjacentIndex(gridContainer.Grid.Size);

                if(!IsEmptyOrInvalidCell(adjacentIndex))
                {
                    Vector2Int directionFromPortToAdjacent = adjacentIndex - portIndex;
                    PieceDirection direction = directionFromPortToAdjacent.ToPieceDirection(); 
                    
                    Piece adjacentPiece = gridContainer.Pieces[adjacentIndex];
                    bool isValidDirection = (direction.Opposite() & adjacentPiece.Direction) != 0;

                    if (isValidDirection)
                    {
                        searchHeads.Enqueue(adjacentIndex);
                    }
                }
            }
        }

        private void SearchHead(Vector2Int head)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector2Int adjacentIndex = head + new Vector2Int(i, j);
                    bool isDiagonal = i != 0 && j != 0;

                    if (isDiagonal || adjacentIndex == head || IsEmptyOrInvalidCell(adjacentIndex))
                    {
                        continue;
                    }

                    Piece headPiece = gridContainer.Pieces[head];
                    Piece adjacentPiece = gridContainer.Pieces[adjacentIndex];

                    Vector2Int directionToAdjacent = adjacentPiece.Index - headPiece.Index;

                    PieceDirection direction = directionToAdjacent.ToPieceDirection();
                    PieceDirection oppositeDirection = direction.Opposite();
                    
                    bool headPieceIsConnected = (headPiece.Direction & direction) != 0;
                    bool adjacentPieceIsConnected = (oppositeDirection & adjacentPiece.Direction) != 0;

                    if (headPieceIsConnected)
                    {
                        if (adjacentPieceIsConnected)
                        {
                            searchHeads.Enqueue(adjacentIndex);
                        }
                        else
                        {
                            OnFlowLeaked(new FlowLeakInformation(headPiece.Index, adjacentPiece.Index));
                        }
                    }
                }
            }
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