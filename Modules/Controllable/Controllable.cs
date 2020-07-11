using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class Controllable : KinematicBody2D
    {
        public Vector2 Velocity = Vector2.Zero;

        [Export] public float MoveSpeed = 50;
        [Export] public bool InControl;

        public override void _PhysicsProcess(float delta)
        {
            if (InControl) HandleMovement();
        }

        private void HandleMovement()
        {
            Velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
            Velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
            Velocity = MoveAndSlide(Velocity.Normalized() * MoveSpeed);
        }
    }
}
