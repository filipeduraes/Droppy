using System.Collections;
using Droppy.Input;
using Droppy.SpawnSystem;
using Droppy.StatSystem;
using Droppy.UI.ViewModel;
using Droppy.VerticalGame;
using Droppy.VerticalScrollerMinigame.LevelController;
using IdeaToGame.PersistenceSystem;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.TestTools.Utils;

namespace Droppy.Tests.PlayMode
{
    public class VerticalScrollerTests
    {
        private const float TestLevelDuration = 0.1f;

        private Stat purityStat;
        private Stat timeStat;

        private GameObject controllerGo;
        private VerticalGameController controller;
        private FakeEndScreenViewModel fakeViewModel;

        private GameObject levelGo;
        private VerticalScrollerLevel level;
        private FakeDroppyInput fakeInput;
        private FakeSpawner fakeSpawner;
        private LevelIntroductionViewModel fakeLevelViewModel;

        [SetUp]
        public void SetUp()
        {
            Persistence.ClearAllData();

            purityStat = Stat.Create("Purity", "Pureza", 100f, 0f, 100f);
            timeStat = Stat.Create("Time", "Tempo", TestLevelDuration);

            fakeViewModel = new FakeEndScreenViewModel();

            controllerGo = new GameObject("VerticalGameController");
            controller = controllerGo.AddComponent<VerticalGameController>();

            SetPrivateField(controller, "purityStat", purityStat);
            SetPrivateField(controller, "timeStat", timeStat);
            SetPrivateField(controller, "secondaryPurityThreshold", 50f);
            SetPrivateField(controller, "tertiaryPurityThreshold", 95f);

            controller.SetEndScreenViewModel(fakeViewModel);
            controller.SetLevelDuration(TestLevelDuration);

            fakeInput = new GameObject("FakeInput").AddComponent<FakeDroppyInput>();
            fakeSpawner = new FakeSpawner();

            fakeLevelViewModel = ScriptableObject.CreateInstance<LevelIntroductionViewModel>();

            levelGo = new GameObject("VerticalScrollerLevel");
            level = levelGo.AddComponent<VerticalScrollerLevel>();

            level.SetLevelIntroduction(fakeLevelViewModel);
            SetPrivateField(level, "controller", controller);

            level.SetDroppyInput(fakeInput);
            level.SetObstacleSpawner(fakeSpawner);
            level.SetTimeBeforeLevelStart(0f);
        }

        [TearDown]
        public void TearDown()
        {
            Object.Destroy(controllerGo);
            Object.Destroy(levelGo);
            Object.Destroy(fakeInput.gameObject);
            Object.DestroyImmediate(fakeLevelViewModel);
            Persistence.ClearAllData();
        }

        // ─── Timer ────────────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator StartTimer_Sets_TimeStat_To_LevelDuration()
        {
            controller.StartTimer();
            yield return null;

            float timeValue = StatManager.Read(timeStat);
            Assert.That(timeValue, Is.LessThanOrEqualTo(TestLevelDuration));
            Assert.That(timeValue, Is.GreaterThanOrEqualTo(0f));
        }

        [UnityTest]
        public IEnumerator StartTimer_Decrements_TimeStat_Each_Frame()
        {
            controller.StartTimer();

            yield return null;
            float valueAfterOneFrame = StatManager.Read(timeStat);

            yield return null;
            float valueAfterTwoFrames = StatManager.Read(timeStat);

            Assert.That(valueAfterTwoFrames, Is.LessThan(valueAfterOneFrame));
        }

        [UnityTest]
        public IEnumerator Timer_Reaching_Zero_Fires_OnLevelFinished()
        {
            bool levelFinished = false;
            controller.OnLevelFinished += () => levelFinished = true;

            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(levelFinished, Is.True);
        }

        [UnityTest]
        public IEnumerator Timer_Reaching_Zero_Requests_Victory()
        {
            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(fakeViewModel.VictoryRequested, Is.True);
            Assert.That(fakeViewModel.DefeatRequested,  Is.False);
        }

        // ─── Pureza / Derrota ─────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator PurityStat_Reaching_Zero_Fires_OnLevelFinished()
        {
            bool levelFinished = false;
            controller.OnLevelFinished += () => levelFinished = true;

            controller.StartTimer();
            yield return null;

            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 0f));
            yield return null;

            Assert.That(levelFinished, Is.True);
        }

        [UnityTest]
        public IEnumerator PurityStat_Reaching_Zero_Requests_Defeat()
        {
            controller.StartTimer();
            yield return null;

            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 0f));
            yield return null;

            Assert.That(fakeViewModel.DefeatRequested,  Is.True);
            Assert.That(fakeViewModel.VictoryRequested, Is.False);
        }

        [UnityTest]
        public IEnumerator PurityStat_Above_Zero_Does_Not_Trigger_Defeat()
        {
            controller.StartTimer();
            yield return null;

            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 1f));
            yield return null;

            Assert.That(fakeViewModel.DefeatRequested, Is.False);
        }

        // ─── Contagem de estrelas ─────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator Victory_With_Purity_Below_Secondary_Threshold_Gives_One_Star()
        {
            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 30f));
            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(fakeViewModel.Stars, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator Victory_With_Purity_At_Secondary_Threshold_Gives_Two_Stars()
        {
            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 50f));
            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(fakeViewModel.Stars, Is.EqualTo(2));
        }

        [UnityTest]
        public IEnumerator Victory_With_Purity_At_Tertiary_Threshold_Gives_Three_Stars()
        {
            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 95f));
            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(fakeViewModel.Stars, Is.EqualTo(3));
        }

        [UnityTest]
        public IEnumerator Victory_With_No_PurityStat_Gives_One_Star()
        {
            SetPrivateField(controller, "purityStat", (Stat)null);
            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(fakeViewModel.Stars, Is.EqualTo(1));
        }

        // ─── StopGameLogic ────────────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator After_GameOver_PurityStat_Changes_Do_Not_Fire_OnLevelFinished_Again()
        {
            int finishedCount = 0;
            controller.OnLevelFinished += () => finishedCount++;

            controller.StartTimer();
            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 0f));
            yield return null;

            Assert.That(finishedCount, Is.EqualTo(1));
        }

        [UnityTest]
        public IEnumerator Defeat_Stops_Timer_Coroutine()
        {
            controller.StartTimer();
            yield return null;

            StatManager.Modify(purityStat, new StatModifier(StatModifierType.Set, 0f));
            yield return null;

            float timeAtDefeat = StatManager.Read(timeStat);
            yield return new WaitForSeconds(0.05f);
            float timeAfterWait = StatManager.Read(timeStat);

            Assert.That(timeAfterWait, Is.EqualTo(timeAtDefeat).Using(FloatEqualityComparer.Instance));
        }

        // ─── VerticalScrollerLevel ────────────────────────────────────────────────

        [UnityTest]
        public IEnumerator StartLevel_Then_FinishIntroduction_Enables_Input_And_Starts_Spawner()
        {
            level.StartLevel();
            fakeLevelViewModel.FinishLevelIntroduction();
            yield return null; // WaitAndStart com timeBeforeLevelStart = 0

            Assert.That(fakeInput.IsEnabled,   Is.True);
            Assert.That(fakeSpawner.IsRunning, Is.True);
        }

        [UnityTest]
        public IEnumerator StartLevel_Then_FinishIntroduction_Starts_Timer()
        {
            level.StartLevel();
            fakeLevelViewModel.FinishLevelIntroduction();
            yield return null;

            // Timer decrementando confirma que StartTimer foi chamado
            float valueAfterOneFrame = StatManager.Read(timeStat);
            yield return null;
            float valueAfterTwoFrames = StatManager.Read(timeStat);

            Assert.That(valueAfterTwoFrames, Is.LessThan(valueAfterOneFrame));
        }

        [UnityTest]
        public IEnumerator OnLevelFinished_Disables_Input_And_Stops_Spawner()
        {
            level.StartLevel();
            fakeLevelViewModel.FinishLevelIntroduction();
            yield return null;

            yield return new WaitForSeconds(TestLevelDuration + 0.2f);

            Assert.That(fakeInput.IsEnabled,   Is.False);
            Assert.That(fakeSpawner.IsRunning, Is.False);
        }

        [UnityTest]
        public IEnumerator Before_StartLevel_Input_Is_Disabled_And_Spawner_Is_Stopped()
        {
            yield return null;

            Assert.That(fakeInput.IsEnabled,   Is.False);
            Assert.That(fakeSpawner.IsRunning, Is.False);
        }

        // ─── Helpers ──────────────────────────────────────────────────────────────

        private static void SetPrivateField(object target, string fieldName, object value)
        {
            var field = target.GetType().GetField(
                fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            field?.SetValue(target, value);
        }

        // ─── Fakes ────────────────────────────────────────────────────────────────

        private class FakeEndScreenViewModel : IEndScreenViewModel
        {
            public bool VictoryRequested { get; private set; }
            public bool DefeatRequested  { get; private set; }
            public int  Stars            { get; private set; }

            public void RequestVictory(EndScreenResultQuotes quotes, int stars)
            {
                VictoryRequested = true;
                Stars = stars;
            }

            public void RequestDefeat(EndScreenResultQuotes quotes)
            {
                DefeatRequested = true;
            }
        }

        private class FakeDroppyInput : MonoBehaviour, IDroppyInput
        {
            public bool IsEnabled { get; private set; }

            public event System.Action<Vector2> OnPointerStarted   = delegate { };
            public event System.Action          OnMoveStarted      = delegate { };
            public event System.Action          OnMoveCanceled     = delegate { };
            public event System.Action          OnJumpStarted      = delegate { };
            public event System.Action          OnJumpCanceled     = delegate { };
            public event System.Action          OnInteractStarted  = delegate { };
            public event System.Action          OnInteractCanceled = delegate { };
            public Vector2 MoveInput => Vector2.zero;

            public void Enable()  => IsEnabled = true;
            public void Disable() => IsEnabled = false;

            public void SendMoveStarted()             => OnMoveStarted();
            public void SendMoveCanceled()            => OnMoveCanceled();
            public void SendJumpStarted()             => OnJumpStarted();
            public void SendJumpCanceled()            => OnJumpCanceled();
            public void SendInteractStarted()         => OnInteractStarted();
            public void SendInteractCanceled()        => OnInteractCanceled();
            public void SendPointerStarted(Vector2 p) => OnPointerStarted(p);
        }

        private class FakeSpawner : ISpawner
        {
            public bool IsRunning { get; private set; }

            public void StartSpawner() => IsRunning = true;
            public void StopSpawner()  => IsRunning = false;
        }
    }
}