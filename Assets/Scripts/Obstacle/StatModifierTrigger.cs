using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;


namespace Droppy.Obstacle
{
    public class StatModifierTrigger : MonoBehaviour, IInteractableArea
    {
        [Header("StatSettings")]
        [SerializeField]
        private Stat statToModify;

        [SerializeField]
        private StatModifier statModifier;

        public void EnterInteraction(GameObject agent)
        {
            StatManager.Modify(statToModify, statModifier);
            gameObject.SetActive(false);

        }

        public void ExitInteraction(GameObject agent)
        {

        }

    }
}