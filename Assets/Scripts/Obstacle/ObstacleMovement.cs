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

        private void Start()
        {
            if (maxHeightMarker != null)
            {
                markerY = maxHeightMarker.position.y;
            }
        }

        private void OnEnable()
        {
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }
            body.velocity = Vector2.up * movementSpeed;
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                body.velocity = Vector2.zero;

                if (anim != null)
                {
                    anim.SetTrigger(deathTriggerName);
                }
                Collider2D col = GetComponent<Collider2D>();
                if (col != null)
                {
                    col.enabled = false;
                }
            }
        }

        public void DeactivateAndReturnToPool()
        {
            body.velocity = Vector2.zero;

            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }

            gameObject.SetActive(false);
        }
    }
}