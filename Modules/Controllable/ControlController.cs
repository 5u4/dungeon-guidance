using Godot;

namespace DungeonGuidance.Modules.Controllable
{
    public class ControlController : Button
    {
        [Signal]
        public delegate void OnControl(Controllable controllable);

        private Controllable _controllable;

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
            Connect("pressed", this, nameof(OnControlPressed));
        }

        private void OnControlPressed()
        {
            _controllable.AnimationPlayer.Play("ShowEmoji");
            if (_controllable.IsPlayer) return;
            EmitSignal(nameof(OnControl), _controllable);
        }
    }
}
