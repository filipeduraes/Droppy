using System.Collections.Generic;
using Droppy.UI.ViewModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.UI
{
    public class EndScreenView : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text endQuoteText;
        [SerializeField] private List<Image> stars;
        [SerializeField] private RectTransform container;
        [SerializeField] private Button retryButton;
        [SerializeField] private Button continueButton;
        
        [Header("Sprites")]
        [SerializeField] private Sprite fullStarSprite;
        [SerializeField] private Sprite emptyStarSprite;

        [Header("View Model")]
        [SerializeField] private EndScreenViewModel viewModel;

        private void OnEnable()
        {
            viewModel.OnShowEndScreenRequested += ShowEndScreen;
            retryButton.onClick.AddListener(RequestRetry);
            continueButton.onClick.AddListener(RequestNextLevel);
            
            container.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            viewModel.OnShowEndScreenRequested -= ShowEndScreen;
            retryButton.onClick.RemoveListener(RequestRetry);
            continueButton.onClick.RemoveListener(RequestNextLevel);
        }

        private void ShowEndScreen()
        {
            Populate();
            container.gameObject.SetActive(true);
        }

        private void Populate()
        {
            endQuoteText.SetText(viewModel.GetRandomQuote());
            
            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].sprite = i < viewModel.StarCount ? fullStarSprite : emptyStarSprite;
            }

            continueButton.enabled = viewModel.IsVictory;
        }
        
        private void RequestRetry()
        {
            viewModel.RequestRetry();
            container.gameObject.SetActive(false);
        }

        private void RequestNextLevel()
        {
            viewModel.RequestNextLevel();
            container.gameObject.SetActive(false);
        }
    }
}