using Godot;

namespace ArrogantCrawler.Modules.Pickable
{
    public class Pickable : Area2D
    {
        [Signal]
        public delegate void OnPickedUp(Pickable pickable, Node by);

        public AnimatedSprite Sprite;
        public bool PickedUp;

        [Export] public string Type;
        [Export] public PackedScene NextScene;

        public override void _Ready()
        {
            Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Connect("body_entered", this, nameof(OnBodyEnterPickable));
        }

        public void OnBodyEnterPickable(Node body)
        {
            EmitSignal(nameof(OnPickedUp), this, body);
        }
    }
}
