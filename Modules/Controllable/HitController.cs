using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class HitController : Node2D
    {
        private Controllable _controllable;

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
        }

        public void Hit(Controllable from)
        {
            GD.Print(_controllable.Name, " is hit by ", from.Name);
        }
    }
}
