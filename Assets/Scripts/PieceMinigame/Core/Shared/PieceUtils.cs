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
    }
}