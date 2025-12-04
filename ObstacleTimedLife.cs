using System.Collections;
using UnityEngine;

namespace Droppy.Obstacle 
{
    public class ObstacleTimedLife : MonoBehaviour
    {
        [Header("Deactivation Settings")]
        [Tooltip("Time in seconds before the object initiates the deactivation animation.")]
        [SerializeField] private float timeToDeactivate = 3f;

        [Header("Animation References")]
        [Tooltip("Reference to the Animator.")]
        [SerializeField] private Animator objectAnimator;
        
        [Tooltip("The name of the animation state to play before deactivation (e.g., 'Bucket_Hide').")]
        [SerializeField] private string hideAnimationState; 

        private Coroutine _deactivationCoroutine; 
        
        private void OnEnable()
        {
            _deactivationCoroutine = StartCoroutine(DeactivationTimer());
        }

        private void OnDisable()
        {
            if (_deactivationCoroutine != null)
            {
                StopCoroutine(_deactivationCoroutine);
                _deactivationCoroutine = null; 
            }
        }

        private IEnumerator DeactivationTimer()
        {
            yield return new WaitForSeconds(timeToDeactivate);

            if (objectAnimator && !string.IsNullOrEmpty(hideAnimationState)) 
            {
                yield return objectAnimator.PlayAnimationAndWait(hideAnimationState);
            }

            gameObject.SetActive(false);
        }
    }
}