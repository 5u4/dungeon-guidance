using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class Controllable : KinematicBody2D
    {
        public ActionLock ActionLock;
        public AnimatedSprite Sprite;
        public Attack AttackController;
        public TargetFindingController TargetFindingController;
        public HitController HitController;
        public ControlController ControlController;

        [Export] public Vector2 Velocity = Vector2.Zero;
        [Export] public float MoveSpeed = 50;
        [Export] public float SightRadius = 50;
        [Export] public bool InControl;
        [Export] public bool IsPlayer;

        public override void _Ready()
        {
            ActionLock = GetNode<ActionLock>("ActionLock");
            Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            AttackController = GetNode<Attack>("Attack");
            TargetFindingController = GetNode<TargetFindingController>("TargetFinding");
            HitController = GetNode<HitController>("HitController");
            ControlController = GetNode<ControlController>("Control");

            ConnectChildrenSignals(this);
        }

        public void ConnectChildrenSignals(Node root)
        {
            foreach (Node node in root.GetChildren())
            {
                if (node.HasMethod("SafeConnect")) node.Call("SafeConnect");
                ConnectChildrenSignals(node);
            }
        }
    }
}
