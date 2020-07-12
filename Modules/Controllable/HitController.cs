using ArrogantCrawler.Scenes;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class HitController : Node2D
    {
        private GameManager _gameManager;
        private Controllable _controllable;
        private Particles2D _damageIndicator;

        public override void _Ready()
        {
            _gameManager = GetNodeOrNull<GameManager>("../../GameManager");
            _controllable = GetNode<Controllable>("..");
            _damageIndicator = GetNode<Particles2D>("DamageIndicator");
        }

        public void Hit(Controllable from)
        {
            HandleKnockBack(from);
            _gameManager?.CameraShake();
            _damageIndicator.Amount = from.Damage;
            _damageIndicator.Emitting = true;
            TakeDamage(from);
            HealthCheck();
        }

        private void TakeDamage(Controllable from)
        {
            _controllable.Health -= from.Damage;
        }

        private void HealthCheck()
        {
            if (_controllable.Health > 0) return;
            _controllable.ActionLock.Lock();
            _controllable.DeathHandler.HandleDeath();
            _gameManager?.LeaveControl();
        }

        private void HandleKnockBack(Controllable from)
        {
            _controllable.Impulse = from.Position.DirectionTo(_controllable.Position) * from.KnockBack;
        }
    }
}
