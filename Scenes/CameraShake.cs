using System;
using Godot;

namespace DungeonGuidance.Scenes
{
    public class CameraShake : Node2D
    {
        private float _trauma;
        private const float TraumaPower = 1.5f;
        private Vector2 _offset;
        private Random _random;
        private OpenSimplexNoise _noise = new OpenSimplexNoise();
        private int _count = 0;
        private Camera2D _camera;

        [Export] public float Decay = 12f;
        [Export] public Vector2 MaxOffset = new Vector2(3, 1);

        public override void _Ready()
        {
            _random = new Random(DateTime.Now.Millisecond);
            _noise.Seed = DateTime.Now.Millisecond;
            _noise.Period = 4;
            _noise.Octaves = 2;
        }

        public override void _Process(float delta)
        {
            if (_camera == null || _trauma <= 0) return;
            _trauma = Math.Max(_trauma - Decay * delta, 0);
            AddShakingEffect();
        }

        public void Shake(Camera2D camera)
        {
            _camera = camera;
            _trauma = Math.Min(_trauma + 2f, 5f);
        }

        private void AddShakingEffect()
        {
            _count++;
            var amount = (float) Math.Pow(_trauma, TraumaPower);
            _offset.x = MaxOffset.x * amount * _noise.GetNoise2d(_noise.Seed * 2, _count);
            _offset.y = MaxOffset.y * amount * _noise.GetNoise2d(_noise.Seed * 3, _count);
            _camera.Offset = _offset;
        }
    }
}
