using DungeonGuidance.Scenes;
using Godot;

namespace DungeonGuidance.Modules.DraggableCamera
{
    public class DraggableCamera : Camera2D
    {
        private TileMap _tileMap;
        private GameManager _gameManager;

        [Export] public Node2D Target;
        [Export] public bool CanMove;

        public override void _Ready()
        {
            _gameManager = GetNodeOrNull<GameManager>("../GameManager");
            _tileMap = GetNode<TileMap>("../Dungeon");
            LimitCamera();
        }

        public override void _Input(InputEvent @event)
        {
            if (!CanMove) return;
            HandleDrag(@event);
        }

        public override void _Process(float delta)
        {
            if (Target == null) return;
            Position = Target.Position;
        }

        private void HandleDrag(InputEvent @event)
        {
            switch (@event)
            {
                case InputEventMouseButton click when click.Pressed && Input.IsActionPressed("ui_drag"):
                    _gameManager?.SetMouseMode(Input.CursorShape.Drag);
                    break;
                case InputEventMouseMotion drag when Input.IsActionPressed("ui_drag"):
                    _gameManager?.SetMouseMode(Input.CursorShape.Move);
                    var margin = GetViewportRect().Size * Zoom / 2;
                    var target = Position - drag.Relative;
                    target.x = Mathf.Clamp(target.x, LimitLeft + margin.x, LimitRight - margin.x);
                    target.y = Mathf.Clamp(target.y, LimitTop + margin.y, LimitBottom - margin.y);
                    Position = target;
                    break;
                default:
                    _gameManager?.SetMouseMode(Input.CursorShape.Arrow);
                    return;
            }
        }

        private void LimitCamera()
        {
            var end = _tileMap.MapToWorld(_tileMap.GetUsedRect().End);
            LimitLeft = 0;
            LimitTop = 0;
            LimitRight = (int) end.x;
            LimitBottom = (int) end.y;
        }
    }
}
