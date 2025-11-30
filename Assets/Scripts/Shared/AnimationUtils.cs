using System.Collections;
using UnityEngine;

namespace Droppy.Shared
{
    public static class AnimationUtils
    {
        public static IEnumerator PlayAnimationAndWait(this Animator animator, string animationState, int layerIndex = 0)
        {
            int animatorStateHash = Animator.StringToHash(animationState);
            
            if (!animator.HasState(layerIndex, animatorStateHash))
            {
                yield break;
            }
            
            animator.Play(animatorStateHash);

            yield return new WaitUntil(IsCorrectAnimationState);
            
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
            yield return new WaitForSeconds(stateInfo.length);
            
            yield break;

            bool IsCorrectAnimationState() => animator.GetCurrentAnimatorStateInfo(layerIndex).IsName(animationState);
        }
    }
}