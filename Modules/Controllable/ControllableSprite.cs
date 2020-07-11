using System;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class ControllableSprite : AnimatedSprite
    {
        private Controllable _controllable;

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
            var facing = Math.Sign(_controllable.Velocity.x);
            if (facing != 0) FlipH = facing != 1;
        }

        private string GetAnimationState()
        {
            return _controllable.Velocity.Equals(Vector2.Zero) ? "idle" : "move";
        }
    }
}
