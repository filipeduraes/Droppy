using System;
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
        [SerializeField] private Stat purityStat;
        [SerializeField] private Stat timeStat;

        [Header("Configurações da Fase")]
        [SerializeField] private float levelDuration = 60f;

        [Header("Objetivos (Pureza)")]
        [SerializeField] private float secondaryPurityThreshold = 50f;
        [SerializeField] private float tertiaryPurityThreshold = 95f;

        public event Action OnLevelFinished = delegate { }; 
        private Coroutine timerCoroutine;
        private IEndScreenViewModel endScreenViewModel;
        private bool timerIsPaused = false;

        private void Awake()
        {
            SetEndScreenViewModel(viewModel);
        }

        private void Start()
        {
            SetLevelDuration(levelDuration);
        }

        private void OnEnable()
        {
            StatManager.OnStatModified += OnStatChanged;
        }

        private void OnDisable()
        {
            StatManager.OnStatModified -= OnStatChanged;
        }

        public void SetLevelDuration(float newLevelDuration)
        {
            levelDuration = newLevelDuration;
            StatManager.Modify(timeStat, new StatModifier(StatModifierType.Set, levelDuration));
        }

        public void SetEndScreenViewModel(IEndScreenViewModel newEndScreenViewModel)
        {
            endScreenViewModel = newEndScreenViewModel;
        }
        
        public void StartTimer()
        {
            timerIsPaused = false;
            
            if (timeStat != null && timerCoroutine == null)
            {
                timerCoroutine = StartCoroutine(LevelTimerRoutine());
            }
        }
        
        public void PauseTimer()
        {
            timerIsPaused = true;
        }

        private IEnumerator LevelTimerRoutine()
        {
            StatModifier setModifier = new(StatModifierType.Set, levelDuration);
            StatManager.Modify(timeStat, setModifier);

            while (StatManager.Read(timeStat) > 0f)
            {
                yield return null;

                StatModifier subModifier = new(StatModifierType.Add, -Time.deltaTime);
                StatManager.Modify(timeStat, subModifier);

                if (timerIsPaused)
                {
                    yield return new WaitUntil(() => !timerIsPaused);
                }
            }

            GameOverWithVictory();
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

        private void StopGameLogic()
        {
            if (timerCoroutine != null)
            {
                StopCoroutine(timerCoroutine);
                timerCoroutine = null;
            }

            StatManager.OnStatModified -= OnStatChanged;
            OnLevelFinished();
        }

        private void GameOverWithVictory()
        {
            StopGameLogic();

            if (purityStat == null)
            {
                endScreenViewModel.RequestVictory(endScreenQuotes, 1);
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

            endScreenViewModel.RequestVictory(endScreenQuotes, starCount);
        }

        private void GameOverWithDefeat()
        {
            StopGameLogic();
            endScreenViewModel.RequestDefeat(endScreenQuotes);
        }
    }
}