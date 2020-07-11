using Godot;

namespace ArrogantCrawler.Modules.Pickable
{
    public class Pickable : Node2D
    {
        [Signal]
        public delegate void OnPickedUp(Pickable pickable, Node by);

        public Area2D Area;
        public AnimatedSprite Sprite;
        public bool PickedUp;

        [Export] public string Type;
        [Export] public PackedScene NextScene;

        public override void _Ready()
        {
            Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Area = GetNode<Area2D>("Area2D");
            Area.Connect("body_entered", this, nameof(OnBodyEnterPickable));
        }

        public void OnBodyEnterPickable(Node body)
        {
            if (Visible) EmitSignal(nameof(OnPickedUp), this, body);
        }
    }
}
