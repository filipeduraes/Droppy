using System;
using System.Collections;
using System.Threading.Tasks;
using Droppy.UI.ViewModel;
using IdeaToGame.PersistenceSystem;
using UnityEngine;

namespace Droppy.LevelSystem
{
    public class PauseCommandsController : MonoBehaviour
    {
        [SerializeField] private PauseScreenViewModel pauseScreenViewModel;
        [SerializeField] private SceneLoader sceneLoader;

        private bool saveFinished = false;
        
        private void OnEnable()
        {
            pauseScreenViewModel.OnMainMenuRequested += ReturnToMainMenu;
            pauseScreenViewModel.OnExitRequested += ExitGame;
        }

        private void OnDisable()
        {
            pauseScreenViewModel.OnMainMenuRequested -= ReturnToMainMenu;
            pauseScreenViewModel.OnExitRequested -= ExitGame;
        }
        
        private void ReturnToMainMenu()
        {
            SaveAndDoAction(sceneLoader.HideAndLoadScene);
        }
        
        private void ExitGame()
        {
            SaveAndDoAction(Application.Quit);
        }

        private void SaveAndDoAction(Action action)
        {
            Task.Run(async () =>
            {
                await Persistence.SaveAtSlotAsync(0);
                saveFinished = true;
            });
            
            StartCoroutine(WaitSaveAndDoAction(action));
        }

        private IEnumerator WaitSaveAndDoAction(Action action)
        {
            yield return new WaitUntil(() => saveFinished);
            action.Invoke();
        }
    }
}
