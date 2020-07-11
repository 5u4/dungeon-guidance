using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class Controllable : KinematicBody2D
    {
        public Vector2 Velocity = Vector2.Zero;
        public Area2D Sight;
        public Controllable Target;

        [Export] public float MoveSpeed = 50;
        [Export] public float SightRadius = 50;
        [Export] public bool InControl;
        [Export] public bool IsPlayer;

        public override void _Ready()
        {
            Sight = GetNode<Area2D>("Sight");
            Sight.AddChild(MakeCircleCollisionShape(SightRadius));
            Sight.Connect("body_entered", this, nameof(OnBodyEnterSight));
            Sight.Connect("body_exited", this, nameof(OnBodyExitSight));
        }

        public override void _PhysicsProcess(float delta)
        {
            if (InControl) HandleActiveMovement();
            else HandlePassiveMovement();
        }

        private void HandleActiveMovement()
        {
            Velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
            Velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
            Move();
        }

        private void HandlePassiveMovement()
        {
            if (Target == null)
            {
                Velocity = Vector2.Zero;
                return;
            }
            Velocity = Position.DirectionTo(Target.Position);
            Move();
        }

        private void Move()
        {
            Velocity = MoveAndSlide(Velocity.Normalized() * MoveSpeed);
        }

        private CollisionShape2D MakeCircleCollisionShape(float radius)
        {
            return new CollisionShape2D {Shape = new CircleShape2D {Radius = radius}};
        }

        private void OnBodyEnterSight(Node body)
        {
            if (!(body is Controllable controllable) || controllable.IsPlayer == IsPlayer) return;
            Target = controllable;
        }

        private void OnBodyExitSight(Node body)
        {
            if (!(body is Controllable controllable) || controllable.IsPlayer == IsPlayer) return;
            Target = null;
        }
    }
}
