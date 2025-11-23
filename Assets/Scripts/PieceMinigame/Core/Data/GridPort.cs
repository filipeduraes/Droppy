using System;
using UnityEngine;

namespace Droppy.PieceMinigame
{
    [Serializable]
    public class GridPort
    {
        [SerializeField] private PieceDirection direction = PieceDirection.Left;
        [SerializeField] private int offset = 0;

        public PieceDirection Direction
        {
            get => direction;
            set => direction = value;
        }

        public int Offset
        {
            get => offset;
            set => offset = value;
        }
    }
}