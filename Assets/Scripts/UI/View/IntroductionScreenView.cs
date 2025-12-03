using System.Collections;
using Droppy.Shared;
using Droppy.UI.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.UI
{
    public class IntroductionScreenView : MonoBehaviour
    {
        [Header("View")] 
        [SerializeField] private RectTransform container;
        [SerializeField] private TMP_Text levelTitleText;
        [SerializeField] private TMP_Text levelDescriptionText;
        [SerializeField] private Image levelImage;
        [SerializeField] private Button continueButton;

        [Header("Animations")] 
        [SerializeField] private Animator animator;
        [SerializeField] private string showAnimationState;
        [SerializeField] private string hideAnimationState;

        [Header("View Model")] 
        [SerializeField] private LevelIntroductionViewModel viewModel;

        private void OnEnable()
        {
            viewModel.OnLevelIntroductionRequested += ShowLevelIntroduction;
            continueButton.onClick.AddListener(FinishLevelIntroduction);
            
            container.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            viewModel.OnLevelIntroductionRequested -= ShowLevelIntroduction;
            continueButton.onClick.RemoveListener(FinishLevelIntroduction);
        }
        
        private void ShowLevelIntroduction()
        {
            levelTitleText.SetText(viewModel.IntroductionData.Title);
            levelDescriptionText.SetText(viewModel.IntroductionData.Description);
            levelImage.sprite = viewModel.IntroductionData.Image;
            
            container.gameObject.SetActive(true);
            animator.Play(showAnimationState);
        }
        
        private void FinishLevelIntroduction()
        {
            StartCoroutine(PlayHideAnimationAndFinish());
        }

        private IEnumerator PlayHideAnimationAndFinish()
        {
            yield return animator.PlayAnimationAndWait(hideAnimationState);
            container.gameObject.SetActive(false);
            viewModel.FinishLevelIntroduction();
        }
    }
}