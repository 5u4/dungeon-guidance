using ArrogantCrawler.Modules.Controllable;
using Godot;

namespace ArrogantCrawler.Scenes
{
    public class Lv3 : Node2D
    {
        private GameManager _gameManager;

        public override void _Ready()
        {
            _gameManager = GetNode<GameManager>("GameManager");
            _gameManager.SetAllControllableEnabled(true);
            ConnectAllControllables();
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
