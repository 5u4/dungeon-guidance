using System;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class Attack : Node2D // TODO: Rename AttackController
    {
        private Controllable _controllable;
        private Area2D _attackTrigger;
        private Area2D _attackRange;
        private CollisionShape2D _attackRangeCollision;
        private float _attackRangeCollisionOriginalX;

        public bool IsAttacking { get; private set; }
        [Export] public bool AlwaysAttacking;

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
            _attackTrigger = GetNode<Area2D>("AttackTrigger");
            _attackRange = GetNode<Area2D>("AttackRange");

            _attackRangeCollision = _attackRange.GetNodeOrNull<CollisionShape2D>("CollisionShape2D");
            if (_attackRangeCollision == null || AlwaysAttacking) return;
            _attackRangeCollision.Disabled = true;
            _attackRangeCollisionOriginalX = _attackRangeCollision.Position.x;
        }

        public void SafeConnect()
        {
            _controllable.Sprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
            _attackRange.Connect("body_entered", this, nameof(OnBodyEnteredAttackRange));
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_controllable.ActionLock.IsLocked || AlwaysAttacking) return;
            ScanForAttack();
        }

        private void ScanForAttack()
        {
            foreach (var body in _attackTrigger.GetOverlappingBodies())
            {
                if (!(body is Controllable controllable) || controllable.IsPlayer == _controllable.IsPlayer) continue;
                InitiateAttack(controllable);
                return;
            }
        }

        private void InitiateAttack(Controllable controllable)
        {
            var facing = Math.Sign(_controllable.Position.DirectionTo(controllable.Position).x);
            if (facing != 0) _controllable.Sprite.FlipH = facing != 1;

            if (_attackRangeCollision != null)
            {
                _attackRangeCollision.Position =
                    new Vector2((_controllable.Sprite.FlipH ? -1 : 1) * _attackRangeCollisionOriginalX, 0);
                _attackRangeCollision.Disabled = false;
            }

            IsAttacking = true;
            _controllable.ActionLock.Lock();
        }

        private void OnAnimationFinished()
        {
            if (_controllable.Sprite.Animation != "attack") return;
            if (_attackRangeCollision != null) _attackRangeCollision.Disabled = true;
            IsAttacking = false;
            _controllable.ActionLock.Unlock();
        }

        private void OnBodyEnteredAttackRange(Node body)
        {
            if (!(body is Controllable controllable) || controllable.IsPlayer == _controllable.IsPlayer) return;
            controllable.HitController.Hit(_controllable);
        }
    }
}
