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
        public AnimationPlayer AnimationPlayer;

        [Export] public Vector2 Velocity = Vector2.Zero;
        [Export] public Vector2 Impulse = Vector2.Zero;
        [Export] public float ImpulseResist = 4;
        [Export] public float MoveSpeed = 50;
        [Export] public float SightRadius = 50;
        [Export] public bool InControl;
        [Export] public bool IsPlayer;
        [Export] public float KnockBack = 150;
        [Export] public int Damage = 1;
        [Export] public int Health = 2;

        public override void _Ready()
        {
            ActionLock = GetNode<ActionLock>("ActionLock");
            Sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            AttackController = GetNode<Attack>("Attack");
            TargetFindingController = GetNode<TargetFindingController>("TargetFinding");
            HitController = GetNode<HitController>("HitController");
            ControlController = GetNode<ControlController>("Control");
            AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

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
