using System;
using System.Collections.Generic;
using ArrogantCrawler.Modules.Controllable;
using Godot;

namespace ArrogantCrawler.Scenes
{
    public class GameManager : Node2D
    {
        private Resource _arrow;
        private Resource _move;
        private Resource _drag;
        private Resource _pointingHand;

        public override void _Ready()
        {
            _arrow = ResourceLoader.Load("res://Assets/cursor-arrow.png");
            _move = ResourceLoader.Load("res://Assets/cursor-move.png");
            _drag = ResourceLoader.Load("res://Assets/cursor-drag.png");
            _pointingHand = ResourceLoader.Load("res://Assets/cursor-pointinghand.png");
            Input.SetCustomMouseCursor(_pointingHand, Input.CursorShape.PointingHand);
            SetMouseMode(Input.CursorShape.Arrow);
        }

        public void SetMouseMode(Input.CursorShape shape)
        {
            switch (shape)
            {
                case Input.CursorShape.Arrow:
                    Input.SetCustomMouseCursor(_arrow);
                    break;
                case Input.CursorShape.Move:
                    Input.SetCustomMouseCursor(_move);
                    break;
                case Input.CursorShape.Drag:
                    Input.SetCustomMouseCursor(_drag);
                    break;
                case Input.CursorShape.Ibeam:
                    break;
                case Input.CursorShape.PointingHand:
                    Input.SetCustomMouseCursor(_pointingHand);
                    break;
                case Input.CursorShape.Cross:
                    break;
                case Input.CursorShape.Wait:
                    break;
                case Input.CursorShape.Busy:
                    break;
                case Input.CursorShape.CanDrop:
                    break;
                case Input.CursorShape.Forbidden:
                    break;
                case Input.CursorShape.Vsize:
                    break;
                case Input.CursorShape.Hsize:
                    break;
                case Input.CursorShape.Bdiagsize:
                    break;
                case Input.CursorShape.Fdiagsize:
                    break;
                case Input.CursorShape.Vsplit:
                    break;
                case Input.CursorShape.Hsplit:
                    break;
                case Input.CursorShape.Help:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(shape), shape, null);
            }
        }

        public List<Controllable> GetControllables()
        {
            var controllables = new List<Controllable>();
            foreach (var node in GetNode("..").GetChildren())
            {
                if (!(node is Controllable controllable)) continue;
                controllables.Add(controllable);
            }

            return controllables;
        }

        public void SetAllControllableEnabled(bool enabled)
        {
            GetControllables().ForEach(controllable => controllable.ControlController.Visible = enabled);
        }
    }
}
