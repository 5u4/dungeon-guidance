using Godot;

namespace ArrogantCrawler.Modules.DraggableCamera
{
    public class DraggableCamera : Camera2D
    {
        private TileMap _tileMap;

        public override void _Ready()
        {
            _tileMap = GetNode<TileMap>("../Dungeon");
            ZoomOut();
            LimitCamera();
        }

        public override void _Input(InputEvent @event)
        {
            if (!(@event is InputEventMouseMotion drag) || !(drag.Pressure > 0) ||
                !Input.IsActionPressed("ui_drag")) return;
            var margin = GetViewportRect().Size / 2 * Zoom;
            var target = Position - drag.Relative;
            target.x = Mathf.Clamp(target.x, LimitLeft + margin.x, LimitRight - margin.x);
            target.y = Mathf.Clamp(target.y, LimitTop + margin.y, LimitBottom - margin.y);
            Position = target;
        }

        public void ZoomOut()
        {
            Zoom = new Vector2(2, 2);
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
