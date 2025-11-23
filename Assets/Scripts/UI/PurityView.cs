using Droppy.StatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.UI
{
    public class PurityView : MonoBehaviour
    {
        [SerializeField] private Stat purityStat;
        [SerializeField] private Slider purityBar;

        private void Awake()
        {
            UpdatePurityBar(purityStat.ID);
        }

        private void OnEnable()
        {
            StatManager.OnStatModified += UpdatePurityBar;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= UpdatePurityBar;
        }

        private void UpdatePurityBar(string statID)
        {
            if (statID == purityStat.ID)
            {
                float purity = StatManager.Read(purityStat);
                purityBar.value = purity / 100.0f;
            }
        }
    }
}