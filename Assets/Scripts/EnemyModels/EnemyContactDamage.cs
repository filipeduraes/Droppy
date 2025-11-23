using UnityEngine;

namespace Droppy
{
    public class EnemyContactDamage : MonoBehaviour
    {
        [SerializeField]
        private EnemyStats enemyStats;

        public float hitCooldown = 0.5f;
        private float lastHitTime;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (Time.time < lastHitTime + hitCooldown || !other.CompareTag("Player"))
            {
                return;
            }

            PlayerStatController playerStats = other.GetComponent<PlayerStatController>();

            if (playerStats != null)
            {
                if (enemyStats.playerHealthDamage > 0)
                {
                    playerStats.TakeDamage(enemyStats.playerHealthDamage);
                }

                if (enemyStats.scoreDeducted > 0)
                {
                    playerStats.DeductScore(enemyStats.scoreDeducted);
                }

                lastHitTime = Time.time;
            }
        }
    }
}