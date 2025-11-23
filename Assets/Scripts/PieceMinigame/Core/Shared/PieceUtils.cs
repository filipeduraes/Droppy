using System;
using System.Collections.Generic;
using Droppy.PieceMinigame.Data;
using UnityEngine;

namespace Droppy.PieceMinigame.Shared
{
    public static class PieceUtils
    {
        public static PieceDirection RotateClockwise(this PieceDirection pieceDirection, int steps)
        {
            if (steps < 0)
            {
                return pieceDirection;
            }

            steps %= 4;
            PieceDirection rotatedDirection = pieceDirection;
            
            for (int i = 0; i < steps; i++)
            {
                rotatedDirection = RotateClockwise(rotatedDirection);
            }

            return rotatedDirection;
        }
        
        public static PieceDirection RotateClockwise(this PieceDirection pieceDirection)
        {
            PieceDirection rotatedDirection = PieceDirection.None;

            if ((pieceDirection & PieceDirection.Right) != 0)
            {
                rotatedDirection |= PieceDirection.Bottom;
            }
            
            if ((pieceDirection & PieceDirection.Bottom) != 0)
            {
                rotatedDirection |= PieceDirection.Left;
            }
            
            if ((pieceDirection & PieceDirection.Left) != 0)
            {
                rotatedDirection |= PieceDirection.Top;
            }
            
            if ((pieceDirection & PieceDirection.Top) != 0)
            {
                rotatedDirection |= PieceDirection.Right;
            }

            return rotatedDirection;
        }

        public static List<Vector3> ToVectors(this PieceDirection pieceDirection)
        {
            List<Vector3> vectors = new();
            
            if ((pieceDirection & PieceDirection.Right) != 0)
            {
                vectors.Add(Vector2.right);
            }
            
            if ((pieceDirection & PieceDirection.Bottom) != 0)
            {
                vectors.Add(Vector2.down);
            }
            
            if ((pieceDirection & PieceDirection.Left) != 0)
            {
                vectors.Add(Vector2.left);
            }
            
            if ((pieceDirection & PieceDirection.Top) != 0)
            {
                vectors.Add(Vector2.up);
            }

            return vectors;
        }

        public static Vector3 ToVector(this PieceDirection pieceDirection)
        {
            return pieceDirection switch
            {
                PieceDirection.Right => Vector3.right,
                PieceDirection.Bottom => Vector3.down,
                PieceDirection.Left => Vector3.left,
                PieceDirection.Top => Vector3.up,
                _ => Vector3.zero
            };
        }

        public static Vector3 ToOppositeVector(this PieceDirection pieceDirection)
        {
            return -pieceDirection.ToVector();
        }

        public static PieceDirection Opposite(this PieceDirection pieceDirection)
        {
            PieceDirection oppositeDirection = PieceDirection.None;
            
            if ((pieceDirection & PieceDirection.Right) != 0)
            {
                oppositeDirection |= PieceDirection.Left;
            }
            
            if ((pieceDirection & PieceDirection.Bottom) != 0)
            {
                oppositeDirection |= PieceDirection.Top;
            }
            
            if ((pieceDirection & PieceDirection.Left) != 0)
            {
                oppositeDirection |= PieceDirection.Right;
            }
            
            if ((pieceDirection & PieceDirection.Top) != 0)
            {
                oppositeDirection |= PieceDirection.Bottom;
            }

            return oppositeDirection;
        }

        public static PieceDirection ToPieceDirection(this Vector2Int direction)
        {
            if (direction == Vector2Int.right)
            {
                return PieceDirection.Right;
            }
            
            if (direction == Vector2Int.down)
            {
                return PieceDirection.Bottom;
            }
            
            if (direction == Vector2Int.left)
            {
                return PieceDirection.Left;
            }
            
            if (direction == Vector2Int.up)
            {
                return PieceDirection.Top;
            }

            return PieceDirection.None;
        }
    }
}