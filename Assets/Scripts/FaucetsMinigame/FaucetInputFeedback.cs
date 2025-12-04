using UnityEngine;

namespace Droppy.FaucetsMinigame.FaucetsMinigame
{
    public class FaucetInputFeedback : MonoBehaviour
    {
        [SerializeField] private Faucet faucet;
        [SerializeField] private Transform root;

        private void OnEnable()
        {
            faucet.OnEnterInteraction += ShowFeedback;
            faucet.OnExitInteraction += HideFeedback;
            
            faucet.OnStartClosing += HideFeedback;
            faucet.OnClosingFailed += ShowFeedback;
            faucet.OnClosed += OnFaucetClosed;
            
            HideFeedback();
        }

        private void OnDisable()
        {
            faucet.OnEnterInteraction -= ShowFeedback;
            faucet.OnExitInteraction -= HideFeedback;
            
            faucet.OnStartClosing -= HideFeedback;
            faucet.OnClosingFailed -= ShowFeedback;
            faucet.OnClosed -= OnFaucetClosed;
        }
        
        private void ShowFeedback()
        {
            if (faucet.IsOpened)
            {
                root.gameObject.SetActive(true);
            }
        }
        
        private void HideFeedback()
        {
            root.gameObject.SetActive(false);
        }
        
        private void OnFaucetClosed(Faucet _)
        {
            HideFeedback();
        }
    }
}