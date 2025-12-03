using System.Collections;
using Droppy.UI.ViewModel;
using UnityEngine;

namespace Droppy.LevelSystem
{
    public class Level : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private LevelIntroductionViewModel viewModel;
        [SerializeField] private LevelIntroductionData data;
        [SerializeField] private float timeBeforeLevelStart = 2.0f;
        
        public void StartLevel()
        {
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
            OnFinishIntroduction();
        }

        protected virtual void OnFinishIntroduction() { }
    }
}