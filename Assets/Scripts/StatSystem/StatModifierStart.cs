using UnityEngine;

namespace Droppy.StatSystem
{
    public class StatModifierStart : MonoBehaviour
    {
        [SerializeField] private Stat stat;
        [SerializeField] private StatModifier modifier;

        private void Start()
        {
            StatManager.Modify(stat, modifier);
        }
    }
}