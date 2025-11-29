using UnityEngine;

namespace Droppy.Movement
{
    public class OutOfBoundsChecker : MonoBehaviour
    {
        [Header("Boundary Settings")]
        [SerializeField] private float safetyMargin = 3.0f; 
        
        private Camera mainCamera;
        
        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            CheckBounds();
        }

        private void CheckBounds()
        {
            float bottomScreenY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y;
            float outOfBoundsY = bottomScreenY - safetyMargin;

            if (transform.position.y < outOfBoundsY)
            {
                ReturnToPool();
            }
        }

        private void ReturnToPool()
        {
            if (TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = Vector2.zero;
            }
            gameObject.SetActive(false);
        }
    }
}