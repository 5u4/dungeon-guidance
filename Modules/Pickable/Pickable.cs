using Godot;

namespace DungeonGuidance.Modules.Pickable
{
    public class Pickable : Area2D
    {
        [Signal]
        public delegate void OnPickedUp(Pickable pickable, Node by);

        public AnimatedSprite Sprite;
        public bool PickedUp;
        public AudioStreamPlayer2D PickedUpAudio;

        [Export] public string Type;
        [Export] public PackedScene NextScene;

        public override void _Ready()
        {
            Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            Connect("body_entered", this, nameof(OnBodyEnterPickable));
            PickedUpAudio = GetNodeOrNull<AudioStreamPlayer2D>("PickedUpAudio");
        }

        public void OnBodyEnterPickable(Node body)
        {
            EmitSignal(nameof(OnPickedUp), this, body);
        }
    }
}
