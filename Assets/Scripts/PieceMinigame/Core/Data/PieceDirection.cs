using System;

namespace Droppy.PieceMinigame.Data
{
    [Flags]
    public enum PieceDirection
    {
        None   = 0,
        Right  = 1 << 0,
        Bottom = 1 << 1,
        Left   = 1 << 2,
        Top    = 1 << 3
    }
}