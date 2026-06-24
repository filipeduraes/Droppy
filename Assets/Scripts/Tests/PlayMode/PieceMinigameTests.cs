using System.Collections;
using System.Collections.Generic;
using Droppy.Input;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Level;
using Droppy.PieceMinigame.Runtime;
using Droppy.UI.ViewModel;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Droppy.Tests.PlayMode
{
    public class PieceMinigameTests
    {
        private const int MinimumStars = 1;
        private const int IntermediateStars = 2;
        private const int MaximumStars = 3;
        private const float DefaultCellSize = 1.0f;
        private const string StartMethodName = "Start";
        private const string FinishIntroductionMethodName = "OnFinishIntroduction";

        private GameObject levelControllerGameObject;
        private PieceMinigameLevelController levelController;
        private FakeFlowController fakeFlowController;
        private FakeGridContainer fakeGridContainer;
        private FakeEndScreenViewModel fakeEndScreenViewModel;

        private GameObject levelGameObject;
        private PieceMinigameLevel level;
        private FakeDroppyInput fakeDroppyInput;

        [SetUp]
        public void SetUp()
        {
            fakeEndScreenViewModel = new FakeEndScreenViewModel();
            fakeFlowController = new FakeFlowController();
            fakeGridContainer = new FakeGridContainer();
            fakeDroppyInput = new FakeDroppyInput();

            levelControllerGameObject = new GameObject(nameof(PieceMinigameLevelController));

            levelController = levelControllerGameObject.AddComponent<PieceMinigameLevelController>();

            levelController.SetFlowController(fakeFlowController);
            levelController.SetGridContainer(fakeGridContainer);
            levelController.SetEndScreenViewModel(fakeEndScreenViewModel);

            levelGameObject = new GameObject(nameof(PieceMinigameLevel));

            level = levelGameObject.AddComponent<PieceMinigameLevel>();

            level.SetFlowController(fakeFlowController);
            level.SetInput(fakeDroppyInput);
        }

        [TearDown]
        public void TearDown()
        {
            Object.DestroyImmediate(levelControllerGameObject);
            Object.DestroyImmediate(levelGameObject);
        }

        [UnityTest]
        public IEnumerator Start_Level_Disables_Input()
        {
            InvokePrivateMethod(level, StartMethodName);
            yield return null;

            Assert.That(fakeDroppyInput.IsEnabled, Is.False);
        }

        [UnityTest]
        public IEnumerator Finish_Introduction_Enables_Input_And_Starts_Flow()
        {
            InvokePrivateMethod(level, FinishIntroductionMethodName);
            yield return null;

            Assert.That(fakeDroppyInput.IsEnabled, Is.True);
            Assert.That(fakeFlowController.FlowStartedWasCalled, Is.True);
        }

        [UnityTest]
        public IEnumerator Flow_Leaked_Requests_Defeat()
        {
            fakeFlowController.SetLeakedState(true);
            fakeFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(fakeEndScreenViewModel.DefeatRequested, Is.True);
            Assert.That(fakeEndScreenViewModel.VictoryRequested, Is.False);
        }

        [UnityTest]
        public IEnumerator Flow_Finished_Without_Leak_And_Incomplete_Goals_Requests_Victory_With_One_Star()
        {
            fakeFlowController.SetLeakedState(false);

            GridData gridData = ScriptableObject.CreateInstance<GridData>();
            gridData.Exits = new List<GridPort> { new GridPort() };
            fakeGridContainer.Grid = gridData;

            FakePiece unfulfilledLockedPiece = new FakePiece();
            unfulfilledLockedPiece.SetLockedState(true);
            fakeGridContainer.Pieces.Add(Vector2Int.zero, unfulfilledLockedPiece);

            fakeFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(fakeEndScreenViewModel.VictoryRequested, Is.True);
            Assert.That(fakeEndScreenViewModel.StarsCount, Is.EqualTo(MinimumStars));
        }

        [UnityTest]
        public IEnumerator Flow_Finished_All_Exits_Visited_And_Incomplete_Pieces_Requests_Victory_With_Two_Stars()
        {
            fakeFlowController.SetLeakedState(false);

            GridData gridData = ScriptableObject.CreateInstance<GridData>();
            GridPort exitPort = new GridPort();
            gridData.Exits = new List<GridPort> { exitPort };
            gridData.SetGridSize(Vector2Int.zero);
            fakeGridContainer.Grid = gridData;

            fakeFlowController.VisitedPorts.Add(exitPort.GetPortIndex(gridData.Size));

            FakePiece unfulfilledLockedPiece = new FakePiece();
            unfulfilledLockedPiece.SetLockedState(true);
            fakeGridContainer.Pieces.Add(Vector2Int.zero, unfulfilledLockedPiece);

            fakeFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(fakeEndScreenViewModel.VictoryRequested, Is.True);
            Assert.That(fakeEndScreenViewModel.StarsCount, Is.EqualTo(IntermediateStars));
        }

        [UnityTest]
        public IEnumerator Flow_Finished_All_Goals_Completed_Requests_Victory_With_Three_Stars()
        {
            fakeFlowController.SetLeakedState(false);

            GridData gridData = ScriptableObject.CreateInstance<GridData>();
            GridPort exitPort = new GridPort();
            gridData.Exits = new List<GridPort> { exitPort };
            gridData.SetGridSize(Vector2Int.zero);
            fakeGridContainer.Grid = gridData;

            fakeFlowController.VisitedPorts.Add(exitPort.GetPortIndex(gridData.Size));

            FakePiece fulfilledLockedPiece = new FakePiece();
            fulfilledLockedPiece.SetLockedState(true);
            fulfilledLockedPiece.Fill();
            fakeGridContainer.Pieces.Add(Vector2Int.zero, fulfilledLockedPiece);

            fakeFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(fakeEndScreenViewModel.VictoryRequested, Is.True);
            Assert.That(fakeEndScreenViewModel.StarsCount, Is.EqualTo(MaximumStars));
        }

        private static void InvokePrivateMethod(object targetInstance, string methodName)
        {
            System.Reflection.MethodInfo methodInformation = targetInstance.GetType().GetMethod(
                methodName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance
            );
            methodInformation?.Invoke(targetInstance, null);
        }

        private class FakeEndScreenViewModel : IEndScreenViewModel
        {
            public bool VictoryRequested { get; private set; }
            public bool DefeatRequested { get; private set; }
            public int StarsCount { get; private set; }

            public void RequestVictory(EndScreenResultQuotes resultQuotes, int starsResultCount)
            {
                VictoryRequested = true;
                StarsCount = starsResultCount;
            }

            public void RequestDefeat(EndScreenResultQuotes resultQuotes)
            {
                DefeatRequested = true;
            }
        }

        private class FakeDroppyInput : IDroppyInput
        {
            public bool IsEnabled { get; private set; }

            public event System.Action<Vector2> OnPointerStarted = delegate { };
            public event System.Action OnMoveStarted = delegate { };
            public event System.Action OnMoveCanceled = delegate { };
            public event System.Action OnJumpStarted = delegate { };
            public event System.Action OnJumpCanceled = delegate { };
            public event System.Action OnInteractStarted = delegate { };
            public event System.Action OnInteractCanceled = delegate { };
            public Vector2 MoveInput => Vector2.zero;

            public void Enable() => IsEnabled = true;
            public void Disable() => IsEnabled = false;

            public void SendMoveStarted() => OnMoveStarted();
            public void SendMoveCanceled() => OnMoveCanceled();
            public void SendJumpStarted() => OnJumpStarted();
            public void SendJumpCanceled() => OnJumpCanceled();
            public void SendInteractStarted() => OnInteractStarted();
            public void SendInteractCanceled() => OnInteractCanceled();
            public void SendPointerStarted(Vector2 pointerPosition) => OnPointerStarted(pointerPosition);
        }

        private class FakeFlowController : IFlowController
        {
            public event System.Action OnFlowUpdate = delegate { };
            public event System.Action OnFlowStarted = delegate { };
            public event System.Action<FlowInformation> OnFlowLeaked = delegate { };
            public event System.Action<FlowInformation> OnPortFlow = delegate { };
            public event System.Action OnFlowFinished = delegate { };

            public bool Leaked { get; private set; }
            public IEnumerable<Vector2Int> Visited { get; set; } = new List<Vector2Int>();
            public HashSet<Vector2Int> VisitedPorts { get; set; } = new HashSet<Vector2Int>();
            public bool FlowStartedWasCalled { get; private set; }

            public void StartFlow() => FlowStartedWasCalled = true;
            public void Stop() { }
            public void TriggerFlowFinished() => OnFlowFinished();
            public void SetLeakedState(bool leakedState) => Leaked = leakedState;
        }

        private class FakeGridContainer : IGridContainer
        {
            public GridData Grid { get; set; }
            public Dictionary<Vector2Int, IPiece> Pieces { get; set; } = new Dictionary<Vector2Int, IPiece>();
            public Dictionary<Vector2Int, SpriteRenderer> Entries { get; set; } = new Dictionary<Vector2Int, SpriteRenderer>();
            public Dictionary<Vector2Int, SpriteRenderer> Exits { get; set; } = new Dictionary<Vector2Int, SpriteRenderer>();
            public float CellSize => DefaultCellSize;

            public Vector3 GetPortBorderPosition(GridPort gridPort) => Vector3.zero;
            public Vector3 GetBorderPosition(int horizontalIndex, int verticalIndex, PieceDirection pieceDirection) => Vector3.zero;
            public Vector3 GetCellCenterPosition(int horizontalIndex, int verticalIndex) => Vector3.zero;
            public Vector3 GetCellPosition(int horizontalIndex, int verticalIndex) => Vector3.zero;
        }

        private class FakePiece : IPiece
        {
            public PieceDirection Direction { get; set; }
            public bool IsFull { get; private set; }
            public bool IsLocked { get; private set; }

            public void Populate(CellData cellData, Vector2Int pieceIndex) { }
            public void Fill() => IsFull = true;
            public void Interact(GameObject agentGameObject) { }
            public void SetLockedState(bool lockedState) => IsLocked = lockedState;
        }
    }
}