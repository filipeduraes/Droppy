using UnityEngine;
using System;

namespace Droppy.Obstacle
{
    public class ObstacleMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float minSpeed = 4f;
        [SerializeField] private float maxSpeed = 7f;

        [Header("Pool Settings")]
        [SerializeField] private Transform maxHeightMarker;

        [Header("Interaction Reference")]
        [SerializeField] private StatModifierTrigger statModifierTrigger;

        [Header("Component References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator anim;

        [Header("Animation Settings")]
        [SerializeField] private string deathTriggerName = "Death";

        private float markerY = 0.0f;
<<<<<<< Updated upstream
        private float currentSpeed;
        
=======
        private Collider2D obstacleCollider;

>>>>>>> Stashed changes
        private void Start()
        {
            if (maxHeightMarker != null)
            {
                markerY = maxHeightMarker.position.y;
            }

            obstacleCollider = GetComponent<Collider2D>();

            if (statModifierTrigger == null)
            {
                statModifierTrigger = GetComponent<StatModifierTrigger>();
            }
        }

        private void OnEnable()
        {
<<<<<<< Updated upstream
            // Sorteia uma velocidade aleatória entre o Mínimo e o Máximo
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            body.velocity = Vector2.up * currentSpeed;
=======
            if (statModifierTrigger != null)
            {
                statModifierTrigger.OnStatApplied += HitByPlayer;
            }

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = true;
            }
            body.velocity = Vector2.up * movementSpeed;
>>>>>>> Stashed changes
        }

        private void OnDisable()
        {
            if (statModifierTrigger != null)
            {
                statModifierTrigger.OnStatApplied -= HitByPlayer;
            }
        }

        private void FixedUpdate()
        {
<<<<<<< Updated upstream
            bool isAboveMarker = currentSpeed > 0.0f && body.position.y >= markerY;    
            bool isBelowMarker = currentSpeed < 0.0f && body.position.y <= markerY;
            
=======
            bool isAboveMarker = movementSpeed > 0.0f && body.position.y >= markerY;
            bool isBelowMarker = movementSpeed < 0.0f && body.position.y <= markerY;

>>>>>>> Stashed changes
            if (isAboveMarker || isBelowMarker)
            {
                DeactivateAndReturnToPool();
            }
        }

        private void HitByPlayer()
        {
            body.velocity = Vector2.zero;

            if (anim != null)
            {
                anim.SetTrigger(deathTriggerName);
            }

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = false;
            }
        }

        public void DeactivateAndReturnToPool()
        {
            body.velocity = Vector2.zero;
            if (statModifierTrigger != null)
            {
                statModifierTrigger.enabled = true;
            }

            gameObject.SetActive(false);
        }
    }
}