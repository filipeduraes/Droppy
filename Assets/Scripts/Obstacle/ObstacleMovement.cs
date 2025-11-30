using UnityEngine;

namespace Droppy.Obstacle
{
    public class ObstacleMovement : MonoBehaviour 
    {
        [Header("Movement Settings")]
        [SerializeField] private float minSpeed = 4f;
        [SerializeField] private float maxSpeed = 7f;

        [Header("Pool Settings")]
        [SerializeField] private Transform maxHeightMarker;

        [Header("Component References")]
        [SerializeField] private Rigidbody2D body;

        private float markerY = 0.0f;
        private float currentSpeed;
        
        private void Start()
        {
            if (maxHeightMarker != null)
            {
                markerY = maxHeightMarker.position.y;
            }
        }

        private void OnEnable()
        {
            // Sorteia uma velocidade aleatória entre o Mínimo e o Máximo
            currentSpeed = Random.Range(minSpeed, maxSpeed);
            body.velocity = Vector2.up * currentSpeed;
        }

        private void FixedUpdate()
        {
            bool isAboveMarker = currentSpeed > 0.0f && body.position.y >= markerY;    
            bool isBelowMarker = currentSpeed < 0.0f && body.position.y <= markerY;
            
            if (isAboveMarker || isBelowMarker)
            {
                body.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
    }
}