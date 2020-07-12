using DungeonGuidance.Modules.Controllable;
using DungeonGuidance.Modules.Pickable;
using Godot;

namespace DungeonGuidance.Scenes
{
    public class Lv3 : Node2D
    {
        private GameManager _gameManager;
        private Pickable _exitButton;
        private Pickable _exit;

        public override void _Ready()
        {
            _gameManager = GetNode<GameManager>("GameManager");
            _gameManager.SetAllControllableEnabled(true);
            _exitButton = GetNode<Pickable>("ExitButton");
            _exitButton.Connect("OnPickedUp", this, nameof(OnExitButtonPickedUp));
            _exit = GetNode<Pickable>("Exit");
            _exit.Visible = false;
            _exit.PickedUp = true;
            _exit.Connect("OnPickedUp", this, nameof(OnExitPickedUp));
            ConnectAllControllables();
        }

        private void OnExitButtonPickedUp(Pickable pickable, Node by)
        {
            if (pickable.Type != "ExitButton" || !PlayerPickable(pickable, by)) return;;
            pickable.PickedUpAudio.Play();
            pickable.Sprite.Play("picked");
            pickable.PickedUp = true;
            _exit.Show();
            _exit.PickedUp = false;
        }

        private void OnExitPickedUp(Pickable pickable, Node by)
        {
            if (pickable.Type != "Exit" || !PlayerPickable(pickable, by)) return;;
            pickable.PickedUpAudio.Play();
            GetTree().ChangeSceneTo(pickable.NextScene);
        }

        private bool PlayerPickable(Pickable pickable, Node node)
        {
            return node is Controllable controllable && controllable.IsPlayer && !pickable.PickedUp;
        }

        private void ConnectAllControllables()
        {
            _gameManager.GetControllables().ForEach(controllable =>
                controllable.ControlController.Connect("OnControl", this, nameof(OnControl)));
        }

        private void OnControl(Controllable controllable)
        {
            _gameManager.Control(controllable);
        }
    }
}
