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
        }

        private void HandleKnockBack(Controllable from)
        {
            _controllable.Impulse = from.Position.DirectionTo(_controllable.Position) * from.KnockBack;
        }
    }
}
