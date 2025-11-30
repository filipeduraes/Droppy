using System.Collections;
using Droppy.Shared;
using UnityEngine;

namespace Droppy.Obstacle
{
    public class ObstacleMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float minSpeed = 4f;
        [SerializeField] private float maxSpeed = 7f;

        [Header("Pool Settings")]
        [SerializeField] private Transform maxHeightMarker;

        [Header("Interaction")]
        [SerializeField] private StatModifierTrigger statModifierTrigger;
        [SerializeField] private Collider2D obstacleCollider;

        [Header("Dependencies")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator animator;

        [Header("Animations")]
        [SerializeField] private string deathAnimationStateName = "Death";

        private bool isDuringDeathSequence = false;
        private float markerY = 0.0f;
        private float currentSpeed;

        private void OnEnable()
        {
            markerY = maxHeightMarker.position.y;
            currentSpeed = Random.Range(minSpeed, maxSpeed);

            statModifierTrigger.OnStatApplied += HitByPlayer;
            statModifierTrigger.enabled = true;
            obstacleCollider.enabled = true;
            
            body.velocity = Vector2.up * currentSpeed;
        }

        private void OnDisable()
        {
            statModifierTrigger.OnStatApplied -= HitByPlayer;
            isDuringDeathSequence = false;
        }

        private void FixedUpdate()
        {
            bool isAboveMarker = currentSpeed > 0.0f && body.position.y >= markerY;
            bool isBelowMarker = currentSpeed < 0.0f && body.position.y <= markerY;
            
            if ((isAboveMarker || isBelowMarker) && !isDuringDeathSequence)
            {
                gameObject.SetActive(false);
            }
        }

        private void HitByPlayer()
        {
            if (!isDuringDeathSequence)
            {
                isDuringDeathSequence = true;
                StartCoroutine(PlayDeathAnimationAndReturnToPool());
            }
        }

        private IEnumerator PlayDeathAnimationAndReturnToPool()
        {
            obstacleCollider.enabled = false;
            
            yield return animator.PlayAnimationAndWait(deathAnimationStateName);
            
            body.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}