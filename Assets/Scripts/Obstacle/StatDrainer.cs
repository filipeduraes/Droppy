using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;


namespace Droppy.Obstacle
{
    public class StatDrainer : MonoBehaviour, IInteractableArea
    {
        [Header("StatSettings")]
        [SerializeField]
        private Stat statToModify;

        [SerializeField]
        private StatModifier statModifier;

        public void EnterInteraction(GameObject agent)
        {
            StatManager.Modify(statToModify, statModifier);

            ReturnToPool();
        }

        public void ExitInteraction(GameObject agent)
        {

        }
        public void ReturnToPool()
        {
            gameObject.SetActive(false);
        }

    }
}