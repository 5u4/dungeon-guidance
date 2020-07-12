using Godot;

namespace DungeonGuidance.Scenes
{
    public class Starting : Node2D
    {
        public override void _Input(InputEvent @event)
        {
            if (!(@event is InputEventKey key) || key.Pressed) return;
            GetTree().ChangeScene("res://Scenes/Lv1MovementTutorial.tscn");
        }
    }
}
