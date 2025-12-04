using System;
using UnityEngine;
using Droppy.UI.ViewModel;
using Droppy.StatSystem;

namespace Droppy.WaterLevel
{
    public class WaterLevelController : MonoBehaviour
    {
        [Header("UI & Quotes")]
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private EndScreenResultQuotes endScreenQuotes;

        [Header("Stats Assets")]
        [SerializeField] private Stat waterStat;
        [SerializeField] private Stat timerStat;

        [Header("Objetivos")]
        [SerializeField] private float twoStarsWaterThreshold = 50f;
        [SerializeField] private float threeStarsWaterThreshold = 80f;

        public event Action OnLevelFinished = delegate { };
        
        private void OnEnable()
        {
            StatManager.OnStatModified += OnStatChanged;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= OnStatChanged;
        }

        private void OnStatChanged(string statID)
        {
            if (waterStat != null && statID == waterStat.ID)
            {
                if (StatManager.Read(waterStat) <= 0f)
                {
                    GameOverWithDefeat();
                }
            }

            if (statID == timerStat.ID)
            {
                if (StatManager.Read(timerStat) <= 0)
                {
                    GameOverWithVictory();
                }
            }
        }

        private void StopGameLogic()
        {
            StatManager.OnStatModified -= OnStatChanged;
            StopAllCoroutines();
            OnLevelFinished();
        }

        private void GameOverWithVictory()
        {
            StopGameLogic();

            float finalWaterLevel = StatManager.Read(waterStat);
            int starCount = 1;
            
            if (finalWaterLevel >= twoStarsWaterThreshold)
            {
                starCount++;
            }
            if (finalWaterLevel >= threeStarsWaterThreshold)
            {
                starCount++;
            }

            viewModel.RequestVictory(endScreenQuotes, starCount);
        }

        private void GameOverWithDefeat()
        {
            StopGameLogic();
            viewModel.RequestDefeat(endScreenQuotes);
        }
    }
}