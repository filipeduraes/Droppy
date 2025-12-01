using Droppy.Level;
using Droppy.UI.ViewModel;
using UnityEngine;

namespace Droppy.UIBridge
{
    public class LevelControlsSender : MonoBehaviour
    {
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private LevelController levelController;

        private void OnEnable()
        {
            viewModel.OnNextLevelRequested += levelController.StartNextLevel;
            viewModel.OnRetryRequested += levelController.RestartLevel;
        }

        private void OnDisable()
        {
            viewModel.OnNextLevelRequested -= levelController.StartNextLevel;
            viewModel.OnRetryRequested -= levelController.RestartLevel;
        }
    }
}