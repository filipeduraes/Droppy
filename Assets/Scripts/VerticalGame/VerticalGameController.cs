using UnityEngine;
using Droppy.UI.ViewModel;
using Droppy.StatSystem;
using System;

namespace Droppy.VerticalGame
{
    public class VerticalGameController : MonoBehaviour
    {
        [Header("UI & Quotes")]
        [SerializeField] private EndScreenViewModel viewModel;
        [SerializeField] private EndScreenResultQuotes endScreenQuotes;

        [Header("Stats Assets")]
        [Tooltip("O Stat que representa a pureza da água.")]
        [SerializeField] private Stat purityStat;

        [Tooltip("Arraste o Stat 'LevelTime' aqui.")]
        [SerializeField] private Stat timeStat;

        [Header("Configurações da Fase")]
        [Tooltip("Tempo total em segundos.")]
        [SerializeField] private float levelDuration = 60f;

        [Header("Objetivos (Pureza)")]
        [SerializeField] private float secondaryPurityThreshold = 50f;
        [SerializeField] private float tertiaryPurityThreshold = 95f;

        private bool isLevelFinished = false;

        private void Start()
        {
            InitializeTimer();
        }

        private void OnEnable()
        {
            StatManager.OnStatModified += OnStatChanged;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= OnStatChanged;
        }

        private void Update()
        {
            if (isLevelFinished) return;
            ProcessTimer();
        }

        private void InitializeTimer()
        {
            if (timeStat != null)
            {
                var modifier = new StatModifier(StatModifierType.Set, levelDuration);
                StatManager.Modify(timeStat, modifier);
            }
        }

        private void ProcessTimer()
        {
            if (timeStat == null) return;

            var modifier = new StatModifier(StatModifierType.Add, -Time.deltaTime);
            StatManager.Modify(timeStat, modifier);

            if (StatManager.Read(timeStat) <= 0f)
            {
                HandleLevelFinished(false);
            }
        }

        private void OnStatChanged(string statID)
        {
            if (isLevelFinished) return;
            if (purityStat != null && statID == purityStat.ID)
            {
                if (StatManager.Read(purityStat) <= 0f)
                {
                    HandleLevelFinished(false);
                }
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
                GameOverWithVictory();
            else
                GameOverWithDefeat();
        }

        private void GameOverWithVictory()
        {
            if (purityStat == null)
            {
                viewModel.RequestVictory(endScreenQuotes, 1);
                return;
            }

            float finalPurity = StatManager.Read(purityStat);
            int starCount = 1;

            if (finalPurity >= secondaryPurityThreshold) starCount++;
            if (finalPurity >= tertiaryPurityThreshold) starCount++;

            viewModel.RequestVictory(endScreenQuotes, starCount);
        }

        private void GameOverWithDefeat()
        {
            viewModel.RequestDefeat(endScreenQuotes);
        }
    }
}