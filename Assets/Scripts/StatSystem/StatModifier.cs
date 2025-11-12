using System;
using UnityEngine;

namespace Droppy.StatSystem
{
    [Serializable]
    public class StatModifier
    {
        [SerializeField] private StatModifierType type = StatModifierType.Add;
        [SerializeField] private float amount = 0.0f;

        public StatModifierType Type => type;
        public float Amount => amount;

        public StatModifier(StatModifierType type, float amount)
        {
            this.type = type;
            this.amount = amount;
        }
    }
}