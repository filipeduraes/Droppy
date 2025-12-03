using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;


namespace Droppy.FaucetsMinigame
{

    public class FaucetController : MonoBehaviour, IHoldInteractable
    {
        [SerializeField] private float requiredHoldTime = 1.2f;

        private float _interactionTimer = 0f;
        private bool _isHolding = false;
        private bool _interactionFinished = false;

        [Header("Referências")]
        [SerializeField] private GameObject water;
        [SerializeField] private StatModifierTime modifier;

        public bool IsOpen { get; private set; } = false;

        public static event System.Action<FaucetController> OnFaucetClosed;


        public void Open()
        {
            if (IsOpen) return;

            IsOpen = true;

            if (water != null)
                water.SetActive(true);

            if (modifier != null)
                modifier.enabled = true;
        }

        public void Close()
        {
            if (!IsOpen) return;

            IsOpen = false;

            if (water != null)
                water.SetActive(false);

            if (modifier != null)
                modifier.enabled = false;

            OnFaucetClosed?.Invoke(this);

        }

        public void StartInteraction(GameObject agent)
        {
            _interactionTimer = 0f;
            _isHolding = true;
            _interactionFinished = false;
        }

        public void EndInteraction(GameObject agent)
        {
            _isHolding = false;

            if (_interactionFinished)
            {
                Close(); 
            }
        }

        private void Update()
        {
            if (_isHolding)
            {
                _interactionTimer += Time.deltaTime;

                if (_interactionTimer >= requiredHoldTime)
                {
                    _interactionFinished = true;
                }
            }
        }

    }
}
