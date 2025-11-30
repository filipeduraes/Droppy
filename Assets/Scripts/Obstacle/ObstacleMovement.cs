using UnityEngine;

namespace Droppy.Obstacle
{
    public class ObstacleMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 5f;

        [Header("Pool Settings")]
        [SerializeField] private Transform maxHeightMarker;

        [Header("Component References")]
        [SerializeField] private Rigidbody2D body;
        [SerializeField] private Animator anim;

        [Header("Animation Settings")]
        [SerializeField] private string deathTriggerName = "Death";

        private float markerY = 0.0f;
        private Collider2D obstacleCollider;

        private void Start()
        {
            if (maxHeightMarker != null)
            {
                markerY = maxHeightMarker.position.y;
            }
            obstacleCollider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            StatModifierTrigger.OnStatModified += HitByPlayer;

            if (obstacleCollider != null)
            {
                obstacleCollider.enabled = true;
            }
            body.velocity = Vector2.up * movementSpeed;
        }

        private void OnDisable()
        {
            StatModifierTrigger.OnStatModified -= HitByPlayer;
        }

        private void FixedUpdate()
        {
            bool isAboveMarker = movementSpeed > 0.0f && body.position.y >= markerY;
            bool isBelowMarker = movementSpeed < 0.0f && body.position.y <= markerY;

            if (isAboveMarker || isBelowMarker)
            {
                DeactivateAndReturnToPool();
            }
        }
        private void HitByPlayer(GameObject agent)
        {
            if (agent.CompareTag("Player"))
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
        }

        public void DeactivateAndReturnToPool()
        {
            body.velocity = Vector2.zero;

            gameObject.SetActive(false);
        }
    }
}