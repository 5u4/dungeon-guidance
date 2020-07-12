using System;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class ControllableSprite : AnimatedSprite
    {
        private Controllable _controllable;
        private readonly Vector2 _impulseTolerance = new Vector2(10, 10);

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
        }

        public override void _Process(float delta)
        {
            HandleHorizontalFlip();
            Play(GetAnimationState());
        }

        private void HandleHorizontalFlip()
        {
            if (_controllable.ActionLock.IsLocked) return;
            var facing = Math.Sign(_controllable.Velocity.x);
            if (facing != 0) FlipH = facing != 1;
        }

        private string GetAnimationState()
        {
            if (_controllable.Impulse.Abs() > _impulseTolerance) return "hurt";
            if (_controllable.AttackController.IsAttacking) return "attack";
            return _controllable.Velocity.Equals(Vector2.Zero) ? "idle" : "move";
        }
    }
}
