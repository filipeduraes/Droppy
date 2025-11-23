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
            body.velocity = Vector2.up * movementSpeed;
        }

        private void FixedUpdate()
        {
            bool isAboveMarker = movementSpeed > 0.0f && body.position.y >= markerY;
            bool isBelowMarker = movementSpeed < 0.0f && body.position.y <= markerY;
            
            if (isAboveMarker || isBelowMarker)
            {
                body.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
    }
}