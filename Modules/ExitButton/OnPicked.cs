using Godot;

namespace DungeonGuidance.Modules.ExitButton
{
    public class OnPicked : Node2D
    {
        private Pickable.Pickable _pickable;

        public override void _Ready()
        {
            _pickable = GetNode<Pickable.Pickable>("..");
        }

        public void OnPickedUp(Node body)
        {
            if (!(body is Controllable.Controllable controllable) || !controllable.IsPlayer ||
                _pickable.PickedUp) return;
            _pickable.Sprite.Play("picked");
            _pickable.PickedUp = true;
        }
    }
}
