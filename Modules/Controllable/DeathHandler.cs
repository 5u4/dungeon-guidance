using Godot;

namespace ArrogantCrawler.Modules.Controllable
{
    public class DeathHandler : Node2D
    {
        private Controllable _controllable;
        private AnimatedSprite _sprite;

        public override void _Ready()
        {
            _controllable = GetNode<Controllable>("..");
            _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
            _sprite.Connect("animation_finished", this, nameof(OnAnimatedSpriteFinished));
        }

        public void HandleDeath()
        {
            _sprite.Show();
            _sprite.Play();
            _controllable.Sprite.Hide();
            _controllable.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
        }

        private void OnAnimatedSpriteFinished()
        {
            _controllable.QueueFree();
        }
    }
}
