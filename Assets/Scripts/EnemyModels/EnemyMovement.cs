using UnityEngine;
using Droppy.StatSystem;

namespace Droppy
{
    public class EnemyMovement : MonoBehaviour
    {
        [Header("Referencias")]
        [SerializeField] private EnemyStats enemyStats;

        [Header("Pontos de Patrulha")]
        [Tooltip("O ponto a (posicao incial)")]
        public Vector3 targetA;

        [Tooltip("O ponto B (posição final) da patrulha horizontal. Se zero, será calculado.")]
        public Vector3 targetB;

        [Header("Parâmetros")]
        [Tooltip("Distância mínima para considerar que o inimigo chegou ao ponto de destino.")]
        [SerializeField] private float arrivalThreshold = 0.1f;

        private Vector3 currentTarget;
        private float moveSpeed;

        private void Start()
        {
            if (enemyStats != null && enemyStats.baseMoveSpeedStat != null)
            {
                moveSpeed = StatManager.Read(enemyStats.baseMoveSpeedStat);
            }
            else
            {
                Debug.LogError("EnemyStats ou baseMoveSpeedStat não está configurado. Usando velocidade padrão de 5.");
                moveSpeed = 5f;
            }

            targetA = transform.position;

            if (targetB == Vector3.zero)
            {
                targetB = transform.position + new Vector3(5f, 0f, 0f);
            }
            currentTarget = targetB;
        }

        private void Update()
        {
            if (moveSpeed <= 0) return;
            transform.position = Vector3.MoveTowards(
                transform.position,
                currentTarget,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, currentTarget) < arrivalThreshold)
            {
                if (currentTarget == targetB)
                {
                    currentTarget = targetA;
                }
                else
                {
                    currentTarget = targetB;
                }
            }
        }
    }
}