using UnityEngine;
using Droppy.UI.ViewModel;
using Droppy.StatSystem;
using System.Collections;

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

        private Coroutine _timerCoroutine;

        private void Start()
        {
            StartTimer();
        }

        private void OnEnable()
        {
            StatManager.OnStatModified += OnStatChanged;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= OnStatChanged;
        }
        private void StartTimer()
        {
            if (timeStat != null)
            {
                _timerCoroutine = StartCoroutine(LevelTimerRoutine());
            }
        }

        private IEnumerator LevelTimerRoutine()
        {
            StatModifier setModifier = new StatModifier(StatModifierType.Set, levelDuration);
            StatManager.Modify(timeStat, setModifier);

            while (StatManager.Read(timeStat) > 0f)
            {
                yield return null;

                StatModifier subModifier = new StatModifier(StatModifierType.Add, -Time.deltaTime);
                StatManager.Modify(timeStat, subModifier);
            }

            GameOverWithDefeat();
        }

        private void OnStatChanged(string statID)
        {
            if (purityStat != null && statID == purityStat.ID)
            {
                if (StatManager.Read(purityStat) <= 0f)
                {
                    GameOverWithDefeat();
                }
            }
        }

        public void ReportGoalReached()
        {
            GameOverWithVictory();
        }
        private void StopGameLogic()
        {
            if (_timerCoroutine != null)
            {
                StopCoroutine(_timerCoroutine);
                _timerCoroutine = null;
            }

            StatManager.OnStatModified -= OnStatChanged;
        }

        private void GameOverWithVictory()
        {
            StopGameLogic();

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
            StopGameLogic();
            viewModel.RequestDefeat(endScreenQuotes);
        }
    }
}