using UnityEngine;
using System.Collections; 

namespace Droppy.StatSystem
{
    public class StatModifierTime : MonoBehaviour
    {
        [Header("Configuração da Estatística")]
        [Tooltip("A estatística que será modificada (ex: Water, Health, Mana).")]
        [SerializeField] private Stat targetStat;

        [Header("Configuração do Modificador")]
        [Tooltip("O tipo de modificador a ser aplicado (Add, Multiply, etc.).")]
        [SerializeField] private StatModifierType modifierType = StatModifierType.Add;
        [Tooltip("O valor que será aplicado a cada intervalo de tempo.")]
        [SerializeField] private float modificationValue = -1f; 

        [Header("Configuração do Tempo")]
        [Tooltip("O tempo (em segundos) entre cada aplicação do modificador.")]
        [SerializeField] private float intervalSeconds = 2.0f;

        private Coroutine modificationCoroutine;


        void OnEnable()
        {
            if (targetStat == null)
            {
                Debug.LogError($"[StatModifierTime] O componente no objeto '{gameObject.name}' não tem a Stat Alvo configurada. Desativando.");
                this.enabled = false;
                return;
            }

            modificationCoroutine = StartCoroutine(ApplyModificationOverTime());
        }

        void OnDisable()
        {
            if (modificationCoroutine != null)
            {
                StopCoroutine(modificationCoroutine);
            }
        }


        private IEnumerator ApplyModificationOverTime()
        {
            while (true)
            {
                yield return new WaitForSeconds(intervalSeconds);

                StatModifier modifier = new StatModifier(modifierType, modificationValue);

                StatManager.Modify(targetStat, modifier);

            }
        }
    }
}