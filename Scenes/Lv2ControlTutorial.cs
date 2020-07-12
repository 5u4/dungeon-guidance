using ArrogantCrawler.Modules.Controllable;
using ArrogantCrawler.Modules.DraggableCamera;
using Godot;

namespace ArrogantCrawler.Scenes
{
    public class Lv2ControlTutorial : Node2D
    {
        private DraggableCamera _camera;
        private Controllable _controllable;
        private AnimationPlayer _animationPlayer;

        public override void _Ready()
        {
            _camera = GetNode<DraggableCamera>("DraggableCamera");
            _controllable = GetNode<Controllable>("Crawler");
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey key && key.IsPressed()) _animationPlayer.PlaybackSpeed = 1;
        }
    }
}
