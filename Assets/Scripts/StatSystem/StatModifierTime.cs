using UnityEngine;
using System.Collections; 

namespace Droppy.StatSystem
{
    public class StatModifierTime : MonoBehaviour
    {
        [SerializeField] private Stat targetStat;
        [SerializeField] private StatModifier statModifier;
        [SerializeField] private float intervalSeconds = 2.0f;

        private Coroutine modificationCoroutine;

        private void OnEnable()
        {
            modificationCoroutine = StartCoroutine(ApplyModificationOverTime());
        }

        private void OnDisable()
        {
            if (modificationCoroutine != null)
            {
                StopCoroutine(modificationCoroutine);
            }
        }

        private IEnumerator ApplyModificationOverTime()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(intervalSeconds);

                StatManager.Modify(targetStat, statModifier);
            }
        }
    }
}