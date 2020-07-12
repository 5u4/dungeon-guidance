using ArrogantCrawler.Scenes;
using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class HitController : Node2D
    {
        private GameManager _gameManager;
        private Controllable _controllable;
        private const float KnockBackDuration = 0.2f;

        public override void _Ready()
        {
            _gameManager = GetNodeOrNull<GameManager>("../../GameManager");
            _controllable = GetNode<Controllable>("..");
        }

        public void Hit(Controllable from)
        {
            HandleKnockBack(from);
            _gameManager?.CameraShake();
        }

        private void HandleKnockBack(Controllable from)
        {
            _controllable.Impulse = from.Position.DirectionTo(_controllable.Position) * from.KnockBack;
        }
    }
}
