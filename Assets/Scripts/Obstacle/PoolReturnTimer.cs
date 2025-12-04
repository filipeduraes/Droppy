using System.Collections;
using Droppy.Shared;
using UnityEngine;

namespace Droppy.Obstacle
{
    public class PoolReturnTimer : MonoBehaviour
    {
        [SerializeField, Min(0)] private float timeToDeactivate = 3f;
        [SerializeField] private string animationStateName = "Vanish";
        [SerializeField] private Animator animator;

        private Coroutine deactivationCoroutine;
        
        private void OnEnable()
        {
            deactivationCoroutine = StartCoroutine(DeactivationTimer());
        }
        
        private void OnDisable()
        {
            if (deactivationCoroutine != null)
            {
                StopCoroutine(deactivationCoroutine);
                deactivationCoroutine = null; 
            }
        }

        private IEnumerator DeactivationTimer()
        {
            yield return new WaitForSeconds(timeToDeactivate);
            yield return animator.PlayAnimationAndWait(animationStateName);
            
            gameObject.SetActive(false);
        }
    }
}