using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class TargetFindingController : Node2D
    {
        private Controllable _controllable;
        private Area2D _sight;
        private Controllable _target;

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
            _sight = GetNode<Area2D>("Sight");
            _sight.AddChild(new CollisionShape2D {Shape = new CircleShape2D {Radius = _controllable.SightRadius}});
        }

        public override void _PhysicsProcess(float delta)
        {
            if (_controllable.ActionLock.IsLocked) return;
            if (_controllable.InControl) HandleActiveMovement();
            else HandleAutoMovement();
        }

        public void SafeConnect()
        {
            _sight.Connect("body_exited", this, nameof(OnBodyExistSight));
        }

        private void HandleActiveMovement()
        {
            _controllable.Velocity.x = Input.GetActionStrength("ui_right") - Input.GetActionStrength("ui_left");
            _controllable.Velocity.y = Input.GetActionStrength("ui_down") - Input.GetActionStrength("ui_up");
            Move();
        }

        private void HandleAutoMovement()
        {
            if (_target == null)
            {
                _controllable.Velocity = Vector2.Zero;
                FindTarget();
                return;
            }

            _controllable.Velocity = _controllable.Position.DirectionTo(_target.Position);
            Move();
        }

        private void Move()
        {
            _controllable.Velocity =
                _controllable.MoveAndSlide(_controllable.Velocity.Normalized() * _controllable.MoveSpeed);
        }

        private void FindTarget()
        {
            foreach (var body in _sight.GetOverlappingBodies())
            {
                if (!(body is Controllable controllable) || controllable.IsPlayer == _controllable.IsPlayer || _target != null) continue;
                _target = controllable;
                return;
            }
        }

        private void OnBodyExistSight(Node body)
        {
            if (body == null || _target == null || body.Name != _target.Name) return;
            _target = null;
        }
    }
}
