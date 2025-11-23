using UnityEngine;

namespace Droppy.PieceMinigame
{
    [CreateAssetMenu(fileName = "PieceData", menuName = "Droppy/Piece Minigame/Piece")]
    public class PieceData : ScriptableObject
    {
        [SerializeField] private Sprite sprite;
        [SerializeField] private PieceDirection defaultDirections;
        [SerializeField] private bool isLocked;

        public Sprite Sprite => sprite;
        public PieceDirection DefaultDirections => defaultDirections;
        public bool IsLocked => isLocked;
    }
}