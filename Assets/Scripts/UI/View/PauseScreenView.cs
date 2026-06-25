using System;
using Droppy.UI.ViewModel;
using UnityEngine;

namespace Droppy.UI
{
    public class PauseScreenView : MonoBehaviour
    {
        [SerializeField] private PauseScreenViewModel viewModel;
        [SerializeField] private RectTransform pauseContainer;

        private void Start()
        {
            pauseContainer.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            viewModel.OnPauseRequested += TogglePause;
        }

        private void OnDisable()
        {
            viewModel.OnPauseRequested -= TogglePause;
        }

        private void TogglePause(bool isPaused)
        {
            pauseContainer.gameObject.SetActive(isPaused);
        }
    }
}