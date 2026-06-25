using System.Collections;
using System.Collections.Generic;
using Droppy.PieceMinigame.Data;
using Droppy.PieceMinigame.Level;
using Droppy.PieceMinigame.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Droppy.Tests.PlayMode
{
    public class PieceMinigameTests
    {
        private enum StarCount
        {
            MinimumStars = 1,
            IntermediateStars = 2,
            MaximumStars = 3
        }
        
        private const float DefaultCellSize = 1.0f;
        private const string StartMethodName = "Start";

        private GameObject levelControllerGameObject;
        private PieceMinigameLevelController levelController;
        private MockFlowController mockFlowController;
        private MockGridContainer mockGridContainer;
        private MockEndScreenViewModel mockEndScreenViewModel;

        private GameObject levelGameObject;
        private PieceMinigameLevel level;
        private MockDroppyInput mockDroppyInput;

        [SetUp]
        public void SetUp()
        {
            mockEndScreenViewModel = new MockEndScreenViewModel();
            mockFlowController = new MockFlowController();
            mockGridContainer = new MockGridContainer();
            mockDroppyInput = new MockDroppyInput();

            levelControllerGameObject = new GameObject(nameof(PieceMinigameLevelController));

            levelController = levelControllerGameObject.AddComponent<PieceMinigameLevelController>();

            levelController.SetFlowController(mockFlowController);
            levelController.SetGridContainer(mockGridContainer);
            levelController.SetEndScreenViewModel(mockEndScreenViewModel);

            levelGameObject = new GameObject(nameof(PieceMinigameLevel));

            level = levelGameObject.AddComponent<PieceMinigameLevel>();

            level.SetFlowController(mockFlowController);
            level.SetInput(mockDroppyInput);
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
            ReflectionUtils.InvokePrivateMethod(level, StartMethodName);
            yield return null;

            Assert.That(mockDroppyInput.IsEnabled, Is.False);
        }

        [UnityTest]
        public IEnumerator Resume_Enables_Input_And_Starts_Flow()
        {
            level.Resume();
            yield return null;

            Assert.That(mockDroppyInput.IsEnabled, Is.True);
            Assert.That(mockFlowController.FlowStartedWasCalled, Is.True);
        }

        [UnityTest]
        public IEnumerator Flow_Leaked_Requests_Defeat()
        {
            mockFlowController.SetLeakedState(true);
            mockFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(mockEndScreenViewModel.DefeatRequested, Is.True);
            Assert.That(mockEndScreenViewModel.VictoryRequested, Is.False);
        }

        [UnityTest]
        public IEnumerator Flow_Finished_Without_Leak_And_Incomplete_Goals_Requests_Victory_With_One_Star()
        {
            mockFlowController.SetLeakedState(false);

            GridData gridData = ScriptableObject.CreateInstance<GridData>();
            gridData.Exits = new List<GridPort> { new GridPort() };
            mockGridContainer.Grid = gridData;

            MockPiece unfulfilledLockedPiece = new MockPiece();
            unfulfilledLockedPiece.SetLockedState(true);
            mockGridContainer.Pieces.Add(Vector2Int.zero, unfulfilledLockedPiece);

            mockFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(mockEndScreenViewModel.VictoryRequested, Is.True);
            Assert.That(mockEndScreenViewModel.StarsCount, Is.EqualTo((int) StarCount.MinimumStars));
        }

        [UnityTest]
        public IEnumerator Flow_Finished_All_Exits_Visited_And_Incomplete_Pieces_Requests_Victory_With_Two_Stars()
        {
            mockFlowController.SetLeakedState(false);

            GridData gridData = ScriptableObject.CreateInstance<GridData>();
            GridPort exitPort = new GridPort();
            gridData.Exits = new List<GridPort> { exitPort };
            gridData.SetGridSize(Vector2Int.zero);
            mockGridContainer.Grid = gridData;

            mockFlowController.VisitedPorts.Add(exitPort.GetPortIndex(gridData.Size));

            MockPiece unfulfilledLockedPiece = new MockPiece();
            unfulfilledLockedPiece.SetLockedState(true);
            mockGridContainer.Pieces.Add(Vector2Int.zero, unfulfilledLockedPiece);

            mockFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(mockEndScreenViewModel.VictoryRequested, Is.True);
            Assert.That(mockEndScreenViewModel.StarsCount, Is.EqualTo((int) StarCount.IntermediateStars));
        }

        [UnityTest]
        public IEnumerator Flow_Finished_All_Goals_Completed_Requests_Victory_With_Three_Stars()
        {
            mockFlowController.SetLeakedState(false);

            GridData gridData = ScriptableObject.CreateInstance<GridData>();
            GridPort exitPort = new GridPort();
            gridData.Exits = new List<GridPort> { exitPort };
            gridData.SetGridSize(Vector2Int.zero);
            mockGridContainer.Grid = gridData;

            mockFlowController.VisitedPorts.Add(exitPort.GetPortIndex(gridData.Size));

            MockPiece fulfilledLockedPiece = new MockPiece();
            fulfilledLockedPiece.SetLockedState(true);
            fulfilledLockedPiece.Fill();
            mockGridContainer.Pieces.Add(Vector2Int.zero, fulfilledLockedPiece);

            mockFlowController.TriggerFlowFinished();
            yield return null;

            Assert.That(mockEndScreenViewModel.VictoryRequested, Is.True);
            Assert.That(mockEndScreenViewModel.StarsCount, Is.EqualTo((int) StarCount.MaximumStars));
        }
        
        private class MockFlowController : IFlowController
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

            public void ResumeFlow() => FlowStartedWasCalled = true;
            public void PauseFlow() { }
            public void Stop() { }
            public void TriggerFlowFinished() => OnFlowFinished();
            public void SetLeakedState(bool leakedState) => Leaked = leakedState;
        }

        private class MockGridContainer : IGridContainer
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

        private class MockPiece : IPiece
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