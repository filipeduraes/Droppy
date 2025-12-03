using System;
using System.Collections;
using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;

namespace Droppy.FaucetsMinigame
{
    public class Faucet : MonoBehaviour, IHoldInteractable
    {
        [Header("Settings")]
        [SerializeField] private float requiredHoldTime = 1.2f;
        [SerializeField] private Animator animator;
        [SerializeField] private string openAnimationState = "Open";
        [SerializeField] private string closeAnimationState = "Close";

        [Header("Dependencies")]
        [SerializeField] private StatModifierTime modifier;

        public event Action OnStartClosing = delegate { };
        public event Action OnClosingFailed = delegate { };
        public event Action<Faucet> OnClosed = delegate { };
        
        private bool isOpened = false;
        private Coroutine holdCoroutine;
        
        public void SetOpen(bool newIsOpened)
        {
            if (isOpened != newIsOpened)
            {
                isOpened = newIsOpened;
                modifier.enabled = isOpened;
                
                string animationState = isOpened ? openAnimationState : closeAnimationState;
                animator.Play(animationState);

                if (!isOpened)
                {
                    OnClosed(this);
                }
            }
        }

        public void StartInteraction(GameObject agent)
        {
            if (isOpened)
            {
                holdCoroutine = StartCoroutine(CloseSequence());
            }
        }

        public void EndInteraction(GameObject agent)
        {
            if (isOpened)
            {
                OnClosingFailed();
                return;
            }
            
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
            }
        }
        
        [ContextMenu("Open")]
        private void Open()
        {
            SetOpen(true);
        }

        private IEnumerator CloseSequence()
        {
            OnStartClosing();
            yield return new WaitForSeconds(requiredHoldTime);
            SetOpen(false);
        } 
    }
}
