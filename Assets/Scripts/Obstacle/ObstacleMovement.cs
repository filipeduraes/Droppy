using UnityEngine;

namespace Droppy.Obstacle
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ObstacleMovement : MonoBehaviour 
    {
        [Header("Movement Settings")]
        [SerializeField]
        private float movementSpeed = 5f;

        [Header("Pool Settings")]
        [SerializeField]
        private Transform maxHeightMarker;

        private Rigidbody2D rb;
        private StatDrainer statDrainer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            statDrainer = GetComponent<StatDrainer>();
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

                if(statDrainer != null)
                {
                    statDrainer.ReturnToPool();
                }
            }
        }
    }
}