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

        [Header("Dependencies")]
        [SerializeField] private GameObject water;
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
                water.SetActive(isOpened);
                modifier.enabled = isOpened;

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

        private IEnumerator CloseSequence()
        {
            OnStartClosing();
            yield return new WaitForSeconds(requiredHoldTime);
            SetOpen(false);
        } 
    }
}
