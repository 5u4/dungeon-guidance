using DungeonGuidance.Scenes;
using Godot;

namespace DungeonGuidance.Modules.Controllable
{
    public class HitController : Node2D
    {
        private GameManager _gameManager;
        private Controllable _controllable;
        private Particles2D _damageIndicator;
        private AnimatedSprite _h1;
        private AnimatedSprite _h2;

        public override void _Ready()
        {
            _gameManager = GetNodeOrNull<GameManager>("../../GameManager");
            _controllable = GetNode<Controllable>("..");
            _damageIndicator = GetNode<Particles2D>("DamageIndicator");
            _h1 = GetNode<AnimatedSprite>("../Heart");
            _h2 = GetNode<AnimatedSprite>("../Heart2");
        }

        public void Hit(Controllable from)
        {
            HandleKnockBack(from);
            _controllable.HurtAudio.Play();
            _gameManager?.CameraShake();
            _damageIndicator.Amount = from.Damage;
            _damageIndicator.Emitting = true;
            TakeDamage(from);
            HandleHeartIndicate();
            HealthCheck();
        }

        private void HandleHeartIndicate()
        {
            switch (_controllable.Health)
            {
                case 2:
                    _h1.Animation = "on";
                    _h2.Animation = "on";
                    break;
                case 1:
                    _h1.Animation = "on";
                    _h2.Animation = "off";
                    break;
                default:
                    _h1.Animation = "off";
                    _h2.Animation = "off";
                    break;
            }
            _controllable.HeartAnimation.Play("Hurt");
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
            _gameManager?.LeaveControl(_controllable);
            if (_controllable.IsPlayer) _gameManager?.GameOverIndicator.Show();
        }

        private void HandleKnockBack(Controllable from)
        {
            _controllable.Impulse = from.Position.DirectionTo(_controllable.Position) * from.KnockBack;
        }
    }
}
