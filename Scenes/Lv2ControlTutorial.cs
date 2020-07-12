using ArrogantCrawler.Modules.Controllable;
using ArrogantCrawler.Modules.DraggableCamera;
using ArrogantCrawler.Modules.Pickable;
using Godot;

namespace ArrogantCrawler.Scenes
{
    public class Lv2ControlTutorial : Node2D
    {
        private DraggableCamera _camera;
        private Controllable _goblin;
        private AnimationPlayer _animationPlayer;
        private GameManager _gameManager;
        private Pickable _exitButton;
        private Pickable _exit;
        private bool _movementTutorialPlayed;

        public override void _Ready()
        {
            _camera = GetNode<DraggableCamera>("DraggableCamera");
            _goblin = GetNode<Controllable>("Goblin");
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            _animationPlayer.Connect("animation_finished", this, nameof(OnAnimationFinished));
            _gameManager = GetNode<GameManager>("GameManager");
            _exitButton = GetNode<Pickable>("ExitButton");
            _exitButton.Connect("OnPickedUp", this, nameof(OnExitButtonPickedUp));
            _exit = GetNode<Pickable>("Exit");
            _exit.Connect("OnPickedUp", this, nameof(OnExitPickedUp));
            _exit.Visible = false;
            _exit.PickedUp = true;
            ConnectAllControllables();
        }

        public override void _Input(InputEvent @event)
        {
            if ((!(@event is InputEventKey key) || !key.IsPressed()) &&
                (!(@event is InputEventMouseButton mouse) || !mouse.Pressed)) return;
            _animationPlayer.PlaybackSpeed = Input.IsActionPressed("ui_fastforward") ? 5 : 1;
        }

        private void OnExitButtonPickedUp(Pickable pickable, Node by)
        {
            if (pickable.Type != "ExitButton" || !PlayerPickable(pickable, by)) return;
            pickable.PickedUpAudio.Play();
            pickable.Sprite.Play("picked");
            pickable.PickedUp = true;
            _exit.Show();
            _exit.PickedUp = false;
        }

        private void OnExitPickedUp(Pickable pickable, Node by)
        {
            if (pickable.Type != "Exit" || !PlayerPickable(pickable, by)) return;
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
            if (_movementTutorialPlayed)
            {
                _gameManager.Control(controllable);
                return;
            }
            _gameManager.SetAllControllableEnabled(false);
            _camera.Target = controllable;
            _camera.CanMove = false;
            _goblin.ControlIndicator.Show();
            _animationPlayer.Play("ControllableTutorial");
            _movementTutorialPlayed = true;
        }

        private void OnAnimationFinished(string animationName)
        {
            if (animationName == "Scene")
                _gameManager.SetAllControllableEnabled(true);
            else if (animationName == "ControllableTutorial")
            {
                _goblin.InControl = true;
                _gameManager.CurrentControllable = _goblin;
            }
        }
    }
}
