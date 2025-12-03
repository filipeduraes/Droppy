using UnityEngine;

namespace Droppy.StatSystem
{
    [CreateAssetMenu(menuName = "Droppy/Stat")]
    public class Stat : ScriptableObject
    {
        [SerializeField] private string id;
        [SerializeField] private string displayName;
        [SerializeField] private float initialAmount;

        [Header("Clamp")] 
        [SerializeField] private bool clampValue = false;
        [SerializeField] private float clampMin = 0.0f;
        [SerializeField] private float clampMax = 100.0f;

        public string ID => id;
        public string DisplayName => displayName;
        public float InitialAmount => initialAmount;

        public bool ClampValue => clampValue;
        public float ClampMin => clampMin;
        public float ClampMax => clampMax;
    }
}