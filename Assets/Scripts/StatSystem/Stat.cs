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

        public static Stat Create(string id, string displayName, float initialAmount)
        {
            return Create(id, displayName, initialAmount, false, 0, 0);
        }
        
        public static Stat Create(string id, string displayName, float initialAmount, float clampMin, float clampMax)
        {
            return Create(id, displayName, initialAmount, false, clampMin, clampMax);
        }
        
        private static Stat Create(string id, string displayName, float initialAmount, bool clampValue, float clampMin, float clampMax)
        {
            Stat created = CreateInstance<Stat>();
            
            created.id = id;
            created.displayName = displayName;
            created.initialAmount = initialAmount;
            created.clampValue = clampValue;
            created.clampMin = clampMin;
            created.clampMax = clampMax;
            
            return created;
        }
    }
}