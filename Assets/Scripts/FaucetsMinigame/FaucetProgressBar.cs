using System.Collections;
using Droppy.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.FaucetsMinigame
{
    public class FaucetProgressBar : MonoBehaviour
    {
        [SerializeField] private Faucet faucet;
        [SerializeField] private Slider slider;
        [SerializeField] private Transform root;
        
        [Header("Animation")]
        [SerializeField] private Animator animator;
        [SerializeField] private string showAnimationState = "Show";
        [SerializeField] private string hideAnimationState = "Hide";

        private void Awake()
        {
            root.gameObject.SetActive(false);
            slider.value = 0f;

            faucet.OnStartClosing += OnStart;
            faucet.OnClosingProgress += OnProgress;
            faucet.OnClosingFailed += OnFail;
            faucet.OnClosed += OnClosedEvent;
        }

        private void OnDestroy()
        {
            faucet.OnStartClosing -= OnStart;
            faucet.OnClosingProgress -= OnProgress;
            faucet.OnClosingFailed -= OnFail;
            faucet.OnClosed -= OnClosedEvent;
        }

        private void OnStart()
        {
            slider.value = 0f;
            root.gameObject.SetActive(true);
        }

        private void OnProgress(float value)
        {
            slider.value = value;
        }

        private void OnFail()
        {
            slider.value = 0f;
            StartCoroutine(HideSlider());
        }

        private void OnClosedEvent(Faucet f)
        {
            slider.value = 1f;
            StartCoroutine(HideSlider());
        }

        private IEnumerator HideSlider()
        {
            yield return animator.PlayAnimationAndWait(hideAnimationState);
            root.gameObject.SetActive(false);
        }
    }
}
