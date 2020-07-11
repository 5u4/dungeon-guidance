using System;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class Attack : Node2D // TODO: Rename AttackController
    {
        private Controllable _controllable;
        private Area2D _attackTrigger;

        public bool IsAttacking { get; private set; }

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
            _attackTrigger = GetNode<Area2D>("AttackTrigger");
            _attackTrigger.AddChild(new CollisionShape2D
                {Shape = new CircleShape2D {Radius = _controllable.AttackTriggerRadius}});
        }

        public void SafeConnect()
        {
            _controllable.Sprite.Connect("animation_finished", this, nameof(OnAnimationFinished));
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_controllable.ActionLock.IsLocked) return;
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
            IsAttacking = true;
            _controllable.ActionLock.Lock();
        }

        private void OnAnimationFinished()
        {
            if (_controllable.Sprite.Animation != "attack") return;
            IsAttacking = false;
            _controllable.ActionLock.Unlock();
        }
    }
}
