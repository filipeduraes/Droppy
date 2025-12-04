using System;
using System.Collections;
using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;

namespace Droppy.FaucetsMinigame
{
    public class Faucet : MonoBehaviour, IHoldInteractable, IEnterInteractableArea, IExitInteractableArea
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
        public event Action<float> OnClosingProgress = delegate { };
        
        public event Action OnEnterInteraction = delegate { };
        public event Action OnExitInteraction = delegate { };

        public bool IsOpened { get; private set; } = false;
        private Coroutine holdCoroutine;

        private void Awake()
        {
            SetOpen(false);
        }

        public void SetOpen(bool newIsOpened)
        {
            IsOpened = newIsOpened;
            modifier.enabled = IsOpened;
            
            string animationState = IsOpened ? openAnimationState : closeAnimationState;
            animator.Play(animationState);

            if (!IsOpened)
            {
                OnClosed(this);
            }
        }

        public void StartInteraction(GameObject agent)
        {
            if (IsOpened)
            {
                holdCoroutine = StartCoroutine(CloseSequence());
            }
        }

        public void EndInteraction(GameObject agent)
        {
            if (IsOpened)
            {
                OnClosingFailed();
            }
            
            if (holdCoroutine != null)
            {
                StopCoroutine(holdCoroutine);
            }
        }
        
        public void EnterInteraction(GameObject agent)
        {
            OnEnterInteraction();
        }

        public void ExitInteraction(GameObject agent)
        {
            OnExitInteraction();
        }
        
        [ContextMenu("Open")]
        private void Open()
        {
            SetOpen(true);
        }

        private IEnumerator CloseSequence()
        {
            OnStartClosing();

            float elapsed = 0f;

            while (elapsed < requiredHoldTime)
            {
                elapsed += Time.deltaTime;

                float progress = Mathf.Clamp01(elapsed / requiredHoldTime);
                OnClosingProgress(progress);

                yield return null;
            }

            SetOpen(false);
        }
    }
}
