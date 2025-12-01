using UnityEngine;
using Droppy.UI.ViewModel;
using Droppy.StatSystem;
using System;

namespace Droppy.VerticalGame
{
    public class VerticalGameController : MonoBehaviour
    {
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private EndScreenResultQuotes endScreenQuotes;
        [SerializeField] private Stat purityStat;

        [Header("Purity Thresholds (Objetivos Adicionais)")]
        [Tooltip("Pureza mínima para ganhar a 2ª Estrela.")]
        [SerializeField] private float secondaryPurityThreshold = 75f;
        [Tooltip("Pureza mínima para ganhar a 3ª Estrela.")]
        [SerializeField] private float tertiaryPurityThreshold = 95f;

        private bool isLevelFinished = false;

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
            if (isLevelFinished || purityStat == null || statID != purityStat.ID)
            {
                return;
            }
            if (StatManager.Read(purityStat) <= 0f)
            {
                HandleLevelFinished(false);
            }
        }

        public void ReportGoalReached()
        {
            HandleLevelFinished(true);
        }


        private void HandleLevelFinished(bool isVictory)
        {
            if (isLevelFinished) return;
            isLevelFinished = true;

            if (isVictory)
            {
                GameOverWithVictory();
            }
            else
            {
                GameOverWithDefeat();
            }
        }

        private void GameOverWithVictory()
        {
            if (purityStat == null)
            {
                Debug.LogError("Purity Stat não atribuído! Impossível calcular estrelas com base na Pureza.");
                viewModel.RequestVictory(endScreenQuotes, 1);
                return;
            }

            float finalPurity = StatManager.Read(purityStat);
            int starCount = 1;

            if (finalPurity >= secondaryPurityThreshold)
            {
                starCount++;
            }
            if (finalPurity >= tertiaryPurityThreshold)
            {
                starCount++;
            }

            viewModel.RequestVictory(endScreenQuotes, starCount);
        }

        private void GameOverWithDefeat()
        {
            viewModel.RequestDefeat(endScreenQuotes);
        }
    }
}