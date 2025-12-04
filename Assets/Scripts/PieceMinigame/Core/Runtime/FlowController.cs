using System;
using System.Collections;
using System.Collections.Generic;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Shared;
using UnityEngine;

namespace Droppy.PieceMinigame.Runtime
{
    public struct FlowInformation
    {
        public Vector2Int headIndex;
        public Vector2Int adjacentIndex;

        public FlowInformation(Vector2Int headIndex, Vector2Int adjacentIndex)
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
        public event Action OnFlowStarted = delegate { };
        public event Action<FlowInformation> OnFlowLeaked = delegate { };
        public event Action<FlowInformation> OnPortFlow = delegate { };
        public event Action OnFlowFinished = delegate { };
        
        public IEnumerable<Vector2Int> Visited => visited;
        public HashSet<Vector2Int> VisitedPorts { get; private set; }
        public bool Leaked { get; private set; }

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
                OnFlowFinished();
            }
        }

        private IEnumerator FlowThroughGrid()
        {
            searchHeads = new Queue<Vector2Int>();
            visited = new HashSet<Vector2Int>();
            VisitedPorts = new HashSet<Vector2Int>();

            yield return new WaitForSeconds(secondsBeforeStartFlow);

            OnFlowStarted();
            
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

            OnFlowFinished();
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
                        VisitedPorts.Add(portIndex);
                    }
                    else
                    {
                        FlowInformation flowInformation = new(portIndex, adjacentIndex);
                        Leak(flowInformation);
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

                    if (isDiagonal || adjacentIndex == head)
                    {
                        continue;
                    }
                    
                    Piece headPiece = gridContainer.Pieces[head];
                    Vector2Int directionToAdjacent = adjacentIndex - head;
                    
                    PieceDirection direction = directionToAdjacent.ToPieceDirection();
                    bool headPieceIsConnected = (headPiece.Direction & direction) != 0;
                    FlowInformation flowInformation = new(head, adjacentIndex);

                    if (!headPieceIsConnected)
                    {
                        continue;
                    }
                    
                    if (IsEmptyOrInvalidCell(adjacentIndex))
                    {
                        if (!gridContainer.Grid.IsPortIndex(adjacentIndex))
                        {
                            Leak(flowInformation);
                        }

                        OnPortFlow(flowInformation);
                        VisitedPorts.Add(adjacentIndex);
                        continue;
                    }

                    Piece adjacentPiece = gridContainer.Pieces[adjacentIndex];
                    PieceDirection oppositeDirection = direction.Opposite();
                    
                    bool adjacentPieceIsConnected = (oppositeDirection & adjacentPiece.Direction) != 0;

                    if (adjacentPieceIsConnected)
                    {
                        if (!visited.Contains(adjacentIndex))
                        {
                            searchHeads.Enqueue(adjacentIndex);
                        }
                    }
                    else
                    {
                        Leak(flowInformation);
                    }
                }
            }
        }
        
        private void Leak(FlowInformation flowInformation)
        {
            Leaked = true;
            OnFlowLeaked(flowInformation);
        }

        private bool IsEmptyOrInvalidCell(Vector2Int adjacentIndex)
        {
            return !gridContainer.Grid.IsValidGridIndex(adjacentIndex) 
                   || !gridContainer.Pieces.ContainsKey(adjacentIndex);
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