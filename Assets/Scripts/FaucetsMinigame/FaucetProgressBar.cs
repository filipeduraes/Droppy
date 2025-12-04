using UnityEngine;
using UnityEngine.UI;

namespace Droppy.FaucetsMinigame
{
    public class FaucetProgressBar : MonoBehaviour
    {
        [SerializeField] private Faucet faucet;
        [SerializeField] private Image fillImage;
        [SerializeField] private GameObject root;

        private void Awake()
        {
            if (faucet == null)
                faucet = GetComponentInParent<Faucet>();

            if (root == null)
                root = fillImage.transform.parent.gameObject;

            root.SetActive(false);
            fillImage.fillAmount = 0f;

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
            fillImage.fillAmount = 0f;
            root.SetActive(true);
        }

        private void OnProgress(float value)
        {
            fillImage.fillAmount = value;
        }

        private void OnFail()
        {
            fillImage.fillAmount = 0f;
            root.SetActive(false);
        }

        private void OnClosedEvent(Faucet f)
        {
            fillImage.fillAmount = 1f;
            root.SetActive(false);
        }
    }
}
