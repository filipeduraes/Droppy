using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;
using System;

namespace Droppy.Obstacle
{
    public class StatModifierTrigger : MonoBehaviour, IInteractableArea
    {
        public Action OnStatApplied;

        [Header("StatSettings")]
        [SerializeField]
        private Stat statToModify;

        [SerializeField]
        private StatModifier statModifier;

        public void EnterInteraction(GameObject agent)
        {
            StatManager.Modify(statToModify, statModifier);
            OnStatApplied?.Invoke();

            enabled = false;
        }

        public void ExitInteraction(GameObject agent)
        {
        }
    }
}