using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class Controllable : KinematicBody2D
    {
        public Vector2 Velocity = Vector2.Zero;

        public ActionLock ActionLock;
        public AnimatedSprite Sprite;
        public Attack AttackController;
        public TargetFindingController TargetFindingController;

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

        // private void ScanTarget()
        // {
        //     foreach (var body in Sight.GetOverlappingBodies())
        //     {
        //         if (!(body is Controllable controllable) || controllable.IsPlayer == IsPlayer || Target != null) return;
        //         Target = controllable;
        //         return;
        //     }
        // }
        //
        // private void OnBodyExitSight(Node body)
        // {
        //     if (body == null || Target == null || body.Name != Target.Name) return;
        //     Target = null;
        //     ScanTarget();
        // }
    }
}
