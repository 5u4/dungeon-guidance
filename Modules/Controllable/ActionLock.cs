using Godot;

namespace DungeonGuidance.Modules.Controllable
{
    public delegate void LockTimeoutCallback();

    public class ActionLock : Timer
    {
        private LockTimeoutCallback _callback;

        public bool IsLocked { get; private set; }

        public override void _Ready()
        {
            Connect("timeout", this, nameof(OnActionLockTimeout));
        }

        public void Lock(float duration = float.PositiveInfinity, LockTimeoutCallback callback = null)
        {
            _callback = callback;
            Start(duration);
            IsLocked = true;
        }

        public void Unlock()
        {
            Stop();
            IsLocked = false;
        }

        private void OnActionLockTimeout()
        {
            _callback?.Invoke();
            IsLocked = false;
            Stop();
        }
    }
}
