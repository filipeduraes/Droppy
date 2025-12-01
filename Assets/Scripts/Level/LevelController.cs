using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Droppy.Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private List<Level> levelSequence;
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1.0f;

        public event Action OnLevelSequenceFinished = delegate { };

        private int currentLevelIndex = -1;
        private Level currentLevel;

        private void Start()
        {
            StartNextLevel();
        }
        
        [ContextMenu("Start Next Level")]
        public void StartNextLevel()
        {
            currentLevelIndex++;
            StartCoroutine(StartLevelRoutine());
        }

        [ContextMenu("Restart Level")]
        public void RestartLevel()
        {
            StartCoroutine(StartLevelRoutine());
        }

        private IEnumerator StartLevelRoutine()
        {
            yield return HideCurrentLevel();

            if (currentLevelIndex >= levelSequence.Count)
            {
                OnLevelSequenceFinished();
                yield break;
            }

            yield return ShowCurrentLevel();

            currentLevel.StartLevel();
        }

        private IEnumerator ShowCurrentLevel()
        {
            AsyncInstantiateOperation<Level> instantiateOperation = InstantiateAsync(levelSequence[currentLevelIndex], 1, transform.position, Quaternion.identity);
            yield return instantiateOperation;
            
            currentLevel = instantiateOperation.Result[0];
            currentLevel.transform.SetParent(transform);
            
            yield return FadeTransition(1.0f, 0.0f);
            fadeImage.gameObject.SetActive(false);
        }

        private IEnumerator HideCurrentLevel()
        {
            if (currentLevel != null)
            {
                fadeImage.gameObject.SetActive(true);
                yield return FadeTransition(0.0f, 1.0f);
                Destroy(currentLevel.gameObject);
            }
            else
            {
                SetFadeImageAlpha(1.0f);
            }
        }

        private IEnumerator FadeTransition(float initialAlpha, float finalAlpha)
        {
            float timer = 0.0f;
            
            SetFadeImageAlpha(initialAlpha);
            
            while (timer < fadeDuration)
            {
                float alpha = Mathf.Lerp(initialAlpha, finalAlpha, timer / fadeDuration);
                SetFadeImageAlpha(alpha);

                timer += Time.deltaTime;
                yield return null;
            }
            
            SetFadeImageAlpha(finalAlpha);
        }

        private void SetFadeImageAlpha(float alpha)
        {
            Color fadeImageColor = fadeImage.color;
            fadeImageColor.a = alpha;
            fadeImage.color = fadeImageColor;
        }
    }
}