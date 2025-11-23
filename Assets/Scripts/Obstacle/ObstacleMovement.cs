using UnityEngine;

namespace Droppy.Obstacle
{
    public class ObstacleMovement : MonoBehaviour 
    {
        [Header("Movement Settings")]
        [SerializeField]
        private float movementSpeed = 5f;

        [Header("Pool Settings")]
        [SerializeField]
        private Transform maxHeightMarker;

        [Header("Component References")]
        [SerializeField]
        private Rigidbody2D rb;

        private void Awake()
        {
           
        }

        private void OnEnable()
        {
            rb.velocity = Vector2.up * movementSpeed;
        }

        private void FixedUpdate()
        {
            if (maxHeightMarker !=null && rb.position.y >= maxHeightMarker.position.y)
            {
                rb.velocity = Vector2.zero;
                gameObject.SetActive(false);

            }
        }
    }
}