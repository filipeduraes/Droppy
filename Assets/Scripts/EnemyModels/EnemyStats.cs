using UnityEngine;
using Droppy.StatSystem;

namespace Droppy
{
    [CreateAssetMenu(menuName = "Droppy/EnemyStats", fileName = "NewEnemyModel")]
    public class EnemyStats : ScriptableObject 
    {
        public string enemyName = "Inimigo Generico";

        [Header("Player Penalties")]
        [Tooltip("Dano direto causado a pureza do personagem(float).")]
        public float playerHealthDamage = 0f;

        [Tooltip("Quantidade de pontos subtraidos (int)")]
        public int scoreDeducted = 0;

        [Header("Droppy Stats Base")]
        [Tooltip("Referência ao Stat que define a base da vida do inimigo")]
        public Stat baseHealthStat;

        [Tooltip("Referencia ao Stat que define a velocidade base do movimento do inimigo")]
        public Stat baseMoveSpeedStat;
    }
}