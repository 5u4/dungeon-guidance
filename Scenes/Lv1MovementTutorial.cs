using ArrogantCrawler.Modules.Controllable;
using ArrogantCrawler.Modules.Pickable;
using Godot;

namespace ArrogantCrawler.Scenes
{
    public class Lv1MovementTutorial : Node2D
    {
        private Pickable _exitButton;
        private Pickable _exit;

        public override void _Ready()
        {
            _exitButton = GetNode<Pickable>("ExitButton");
            _exitButton.Connect("OnPickedUp", this, nameof(OnExitButtonPickedUp));
            _exit = GetNode<Pickable>("Exit");
            _exit.Connect("OnPickedUp", this, nameof(OnExitPickedUp));
            _exit.Visible = false;
            _exit.PickedUp = true;
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
    }
}
