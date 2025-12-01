using Droppy.StatSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.UI
{
    public class StatSliderView : MonoBehaviour
    {
        [SerializeField] private Stat stat;
        [SerializeField] private Slider slider;

        private void OnEnable()
        {
            StatManager.OnStatModified += UpdatePurityBar;
            UpdatePurityBar(stat.ID);
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= UpdatePurityBar;
        }

        private void UpdatePurityBar(string statID)
        {
            if (statID == stat.ID)
            {
                float purity = StatManager.Read(stat);
                slider.value = purity / stat.InitialAmount;
            }
        }
    }
}