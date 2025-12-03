using System.Collections;
using Droppy.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Droppy.LevelSystem
{
    public class SceneLoader : MonoBehaviour 
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1.0f;
        [SerializeField] private int sceneBuildIndex = 0;

        public void HideAndLoadScene()
        {
            StartCoroutine(HideAndLoadSceneCoroutine());
        }

        private IEnumerator HideAndLoadSceneCoroutine()
        {
            yield return fadeImage.InterpolateAlpha(0.0f, 1.0f, fadeDuration);
            SceneManager.LoadScene(sceneBuildIndex);
        }
    }
}