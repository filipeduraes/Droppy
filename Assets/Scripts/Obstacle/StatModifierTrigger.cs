using UnityEngine;
using Droppy.StatSystem;
using Droppy.InteractionSystem;

namespace Droppy.Obstacle
{
    public class StatModifierTrigger : MonoBehaviour, IEnterInteractableArea
    {
        [Header("Stat Settings")]
        [SerializeField] private Stat statToModify;
        [SerializeField] private StatModifier statModifier;
        
        public void EnterInteraction(GameObject agent)
        {
            StatManager.Modify(statToModify, statModifier);
            enabled = false;
        }
    }
}