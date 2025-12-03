using System;
using UnityEngine;

namespace Droppy.UI.ViewModel
{
    [CreateAssetMenu(fileName = "LevelIntroductionViewModel", menuName = "Droppy/View Model/Level Introduction", order = 0)]
    public class LevelIntroductionViewModel : ScriptableObject
    {
        [SerializeField] private LevelIntroductionData introductionData;

        public event Action OnLevelIntroductionRequested = delegate { };
        public event Action OnLevelIntroductionFinished = delegate { };
        
        public LevelIntroductionData IntroductionData => introductionData;

        public void StartLevelIntroduction(LevelIntroductionData data)
        {
            introductionData = data;
            OnLevelIntroductionRequested();
        }

        public void FinishLevelIntroduction()
        {
            OnLevelIntroductionFinished();
        }
    }
}