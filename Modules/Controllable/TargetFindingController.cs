using System;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class TargetFindingController : Node2D
    {
        private Controllable _controllable;
        private Area2D _sight;
        private Node2D _target;

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
            _sight = GetNode<Area2D>("Sight");
            _sight.AddChild(new CollisionShape2D {Shape = new CircleShape2D {Radius = _controllable.SightRadius}});
        }

        public override void _PhysicsProcess(float delta)
        {
            Move(delta);
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
        }

        private void HandleAutoMovement()
        {
            if (_target is Pickable.Pickable pickable && pickable.PickedUp) _target = null;
            if (_target == null)
            {
                _controllable.Velocity = Vector2.Zero;
                FindTarget();
                return;
            }

            _controllable.Velocity = _controllable.Position.DirectionTo(_target.Position);
        }

        private void Move(float delta)
        {
            _controllable.Velocity =
                _controllable.MoveAndSlide(_controllable.Velocity.Normalized() * _controllable.MoveSpeed +
                                           _controllable.Impulse);
            _controllable.Impulse.x = Mathf.Lerp(_controllable.Impulse.x, 0, delta * _controllable.ImpulseResist);
            _controllable.Impulse.y = Mathf.Lerp(_controllable.Impulse.y, 0, delta * _controllable.ImpulseResist);
        }

        private void FindTarget()
        {
            // TODO Filter then sort maybe?
            foreach (var body in _sight.GetOverlappingBodies())
            {
                if (!(body is Controllable controllable) ||
                    controllable.IsPlayer == _controllable.IsPlayer || _target != null) continue;
                _target = controllable;
                return;
            }

            foreach (var area in _sight.GetOverlappingAreas())
            {
                if (!(area is Pickable.Pickable pickable) || !_controllable.IsPlayer || _target != null ||
                    pickable.PickedUp) continue;
                _target = pickable;
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
