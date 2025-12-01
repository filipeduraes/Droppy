using UnityEngine;
using System.Collections;
using Droppy.InteractionSystem;
using Droppy.Shared;

namespace Droppy.Obstacle
{
    public class PoolReturnTrigger : MonoBehaviour, IEnterInteractableArea
    {
        [SerializeField] private string animationStateName = "StartAnimation";
        [SerializeField] private Animator animator;
        [SerializeField] private Collider2D objectCollider;

        public void EnterInteraction(GameObject agent)
        {
            StartCoroutine(PlayVanishAnimationAndReturnToPool());
        }
        
        private IEnumerator PlayVanishAnimationAndReturnToPool()
        {
            objectCollider.enabled = false;
            
            yield return animator.PlayAnimationAndWait(animationStateName);
            
            gameObject.SetActive(false);
            objectCollider.enabled = true;
        }
    }
}