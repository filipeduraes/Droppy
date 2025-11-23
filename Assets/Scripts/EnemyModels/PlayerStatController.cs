using UnityEngine;
using Droppy.StatSystem;

namespace Droppy
{
    public class PlayerStatController : MonoBehaviour 
    {
        [Header("Stats do Jogador")]
        [Tooltip("Referência ao ScriptableObject 'Stat' da vida (Pureza).")]
        public Stat healthStat;

        [Tooltip("Referência ao ScriptableObject 'Stat' da pontuação.")]
        public Stat scoreStat;

        public void TakeDamage(float damageAmount)
        {
            if (healthStat == null || damageAmount <= 0) return;

            StatModifier damageModifier = new StatModifier(StatModifierType.Add, -damageAmount);

            StatManager.Modify(healthStat, damageModifier);

            CheckForDeath();

            Debug.Log($"Dano aplicado: {damageAmount}. Nova vida: {StatManager.Read(healthStat)}");
        }

        public void DeductScore(int scoreDeduction)
        {
            if (scoreStat == null || scoreDeduction <= 0) return;

            StatModifier deductionModifier = new StatModifier(StatModifierType.Add, -scoreDeduction);

            StatManager.Modify(scoreStat, deductionModifier);

            Debug.Log($"Pontos deduzidos: {scoreDeduction}. Nova pontuação: {StatManager.Read(scoreStat)}");
        }

        private void CheckForDeath()
        {
            if (StatManager.Read(healthStat) <= 0)
            {
                Debug.Log("Jogador Morreu! Game Over.");
            }
        }
    }
}