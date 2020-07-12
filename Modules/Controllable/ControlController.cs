using Godot;

namespace ArrogantCrawler.Modules.Controllable
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
            if (_controllable.IsPlayer) return; // TODO: Show emoji
            EmitSignal(nameof(OnControl), _controllable);
        }
    }
}
