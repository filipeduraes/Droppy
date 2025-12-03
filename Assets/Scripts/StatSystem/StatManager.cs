using System;
using IdeaToGame.PersistenceSystem;
using UnityEngine;

namespace Droppy.StatSystem
{
    public static class StatManager
    {
        public static event Action<string> OnStatModified = delegate { };

        public static float Read(Stat stat)
        {
            return Persistence.GetData(GetStatKey(stat), stat.InitialAmount);
        }

        public static void Modify(Stat stat, StatModifier modifier)
        {
            float currentStatValue = Read(stat);

            float finalStatValue = modifier.Type switch
            {
                StatModifierType.Reset => stat.InitialAmount,
                StatModifierType.Set => modifier.Amount,
                StatModifierType.Add => currentStatValue + modifier.Amount,
                StatModifierType.Multiply => currentStatValue * modifier.Amount,
                _ => currentStatValue
            };

            if (stat.ClampValue)
            {
                finalStatValue = Mathf.Clamp(finalStatValue, stat.ClampMin, stat.ClampMax);
            }
            
            Persistence.StoreData(GetStatKey(stat), finalStatValue);
            OnStatModified(stat.ID);
        }
        
        private static string GetStatKey(Stat stat)
        {
            return $"StatSystem.{stat.ID}";
        }
    }
}
