using Droppy.StatSystem;
using TMPro;
using UnityEngine;

namespace Droppy.UI
{
    public class TimerView : MonoBehaviour
    {
        [SerializeField] private Stat timeStat;
        [SerializeField] private TMP_Text timeText;

        private void OnEnable()
        {
            StatManager.OnStatModified += UpdateTime;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= UpdateTime;
        }
        
        private void UpdateTime(string statID)
        {
            if (statID == timeStat.ID)
            {
                float currentTime = StatManager.Read(timeStat);
                timeText.SetText($"{currentTime:00}s");
            }
        }
    }
}