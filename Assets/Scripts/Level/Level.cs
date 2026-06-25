using System.Collections;
using Droppy.UI.ViewModel;
using UnityEngine;

namespace Droppy.LevelSystem
{
    public class Level : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private PauseScreenViewModel pauseScreenViewModel;
        [SerializeField] private LevelIntroductionViewModel viewModel;
        [SerializeField] private LevelIntroductionData data;
        [SerializeField] private float timeBeforeLevelStart = 2.0f;
        
        protected virtual void Awake()
        {
            SetLevelIntroduction(viewModel);
        }
        
        private void OnEnable()
        {
            pauseScreenViewModel.OnPauseRequested += TogglePause;
        }

        private void OnDisable()
        {
            pauseScreenViewModel.OnPauseRequested -= TogglePause;
        }

        public void SetLevelIntroduction(LevelIntroductionViewModel newViewModel)
        {
            viewModel = newViewModel;
        }
        
        public void SetTimeBeforeLevelStart(float newTimeBeforeLevelStart)
        {
            timeBeforeLevelStart = newTimeBeforeLevelStart;
        }
        
        public void StartLevel()
        {
            pauseScreenViewModel.SetIsPauseEnabled(false);
            viewModel.StartLevelIntroduction(data);
            viewModel.OnLevelIntroductionFinished += FinishIntroduction;
        }

        private void FinishIntroduction()
        {
            viewModel.OnLevelIntroductionFinished -= FinishIntroduction;

            StartCoroutine(WaitAndStart());
        }

        private IEnumerator WaitAndStart()
        {
            yield return new WaitForSeconds(timeBeforeLevelStart);
            pauseScreenViewModel.SetIsPauseEnabled(true);
            Resume();
        }

        public virtual void Pause(){}

        public virtual void Resume(){}
        
        private void TogglePause(bool isPaused)
        {
            if (isPaused)
            {
                Time.timeScale = 0.0f;
                Pause();
            }
            else
            {
                Time.timeScale = 1.0f;
                Resume();
            }
        }
    }
}