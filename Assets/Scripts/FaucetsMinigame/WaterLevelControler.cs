using UnityEngine;
using Droppy.UI.ViewModel;
using Droppy.StatSystem;
using System.Collections;

namespace Droppy.WaterLevel
{
    public class WaterLevelController : MonoBehaviour
    {
        [Header("UI & Quotes")]
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private EndScreenResultQuotes endScreenQuotes;

        [Header("Stats Assets")]
        [Tooltip("O Stat que representa o nível da água.")]
        [SerializeField] private Stat waterStat;

        [Header("Objetivos (Nível de Água)")]
        [SerializeField] private float twoStarsWaterThreshold = 50f;
        [SerializeField] private float threeStarsWaterThreshold = 80f;

        private bool _isGameRunning = true;

        private void OnEnable()
        {
            _isGameRunning = true;
            StatManager.OnStatModified += OnStatChanged;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= OnStatChanged;
        }

        private void OnStatChanged(string statID)
        {
            if (!_isGameRunning) return;

            if (waterStat != null && statID == waterStat.ID)
            {
                if (StatManager.Read(waterStat) <= 0f)
                {
                    GameOverWithDefeat();
                }
            }
        }

        public void FinishLevel()
        {
            if (_isGameRunning)
            {
                GameOverWithVictory();
            }
        }

        private void StopGameLogic()
        {
            _isGameRunning = false;
            StatManager.OnStatModified -= OnStatChanged;
            StopAllCoroutines();
        }

        private void GameOverWithVictory()
        {
            StopGameLogic();

            if (waterStat == null)
            {
                viewModel.RequestVictory(endScreenQuotes, 1);
                return;
            }

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