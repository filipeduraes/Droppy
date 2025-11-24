using System;
using UnityEngine;

namespace Droppy.PieceMinigame.Data
{
    [Serializable]
    public class CellData
    {
        [SerializeField] private PieceData piece = null;
        
        [Range(0, 3)]
        [SerializeField] private int rotationSteps = 0;

        public PieceData Piece
        {
            get => piece;
            set => piece = value;
        }

        public int RotationSteps
        {
            get => rotationSteps;
            set => rotationSteps = value;
        }
    }
}