using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;
using System;

namespace Droppy.Obstacle
{
    public class StatModifierTrigger : MonoBehaviour, IEnterInteractableArea
    {
        [Header("Stat Settings")]
        [SerializeField] private Stat statToModify;

        [SerializeField] private StatModifier statModifier;
        
        public event Action OnStatApplied = delegate { };

        public void EnterInteraction(GameObject agent)
        {
            StatManager.Modify(statToModify, statModifier);
            OnStatApplied();

            enabled = false;
        }
    }
}