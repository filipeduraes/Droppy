using System;
using Droppy.Input;
using UnityEngine;

namespace Droppy.Tests.PlayMode
{
    public class MockDroppyInput : IDroppyInput
    {
        public event Action<Vector2> OnPointerStarted = delegate { };
        public event Action OnMoveStarted = delegate { };
        public event Action OnMoveCanceled = delegate { };
        public event Action OnJumpStarted = delegate { };
        public event Action OnJumpCanceled = delegate { };
        public event Action OnInteractStarted = delegate { };
        public event Action OnInteractCanceled = delegate { };
        public event Action OnPauseStarted = delegate { };
        
        public bool IsEnabled { get; private set; }
        public Vector2 MoveInput => Vector2.zero;

        public void Enable()  => IsEnabled = true;
        public void Disable() => IsEnabled = false;

        public void SendMoveStarted() => OnMoveStarted();
        public void SendMoveCanceled() => OnMoveCanceled();
        public void SendJumpStarted() => OnJumpStarted();
        public void SendJumpCanceled() => OnJumpCanceled();
        public void SendInteractStarted() => OnInteractStarted();
        public void SendInteractCanceled() => OnInteractCanceled();
        public void SendPauseStarted() => OnPauseStarted();
        public void SendPointerStarted(Vector2 p) => OnPointerStarted(p);
    }
}