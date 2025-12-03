using System.Collections;
using UnityEngine;

namespace Droppy.Obstacle
{
    public class TimedDeactivator : MonoBehaviour
    {
        [Header("Deactivation Settings")]
        [Tooltip("Time in seconds before the object is deactivated.")]
        [SerializeField] private float timeToDeactivate = 3f;

        [Header("References (Optional)")]
        [Tooltip("Reference to the Animator for a disappearing animation.")]
        [SerializeField] private Animator objectAnimator;

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
            
            gameObject.SetActive(false);
        }
    }
}