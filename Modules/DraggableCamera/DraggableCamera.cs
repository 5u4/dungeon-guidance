using Godot;

namespace ArrogantCrawler.Modules.DraggableCamera
{
    public class DraggableCamera : Camera2D
    {
        private TileMap _tileMap;

        [Export] public Node2D Target;
        [Export] public bool CanMove;

        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("../Dungeon");
            LimitCamera();
        }

        public override void _Input(InputEvent @event)
        {
            if (!CanMove) return;
            if (!(@event is InputEventMouseMotion drag) || !(drag.Pressure > 0) ||
                !Input.IsActionPressed("ui_drag")) return;
            var margin = GetViewportRect().Size * Zoom / 2;
            var target = Position - drag.Relative;
            target.x = Mathf.Clamp(target.x, LimitLeft + margin.x, LimitRight - margin.x);
            target.y = Mathf.Clamp(target.y, LimitTop + margin.y, LimitBottom - margin.y);
            Position = target;
        }

        public override void _Process(float delta)
        {
            if (Target == null) return;
            Position = Target.Position;
        }

        public void ZoomOut()
        {
            Zoom = new Vector2(1.5f, 1.5f);
        }

        public void ZoomIn()
        {
            Zoom = new Vector2(1, 1);
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
