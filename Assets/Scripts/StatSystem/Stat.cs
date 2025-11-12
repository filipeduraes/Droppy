using UnityEngine;

namespace Droppy.StatSystem
{
    [CreateAssetMenu(menuName = "Droppy/Stat")]
    public class Stat : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private float initialAmount;

        public string ID => id;
        public string DisplayName => displayName;
        public float InitialAmount => initialAmount;
    }
}