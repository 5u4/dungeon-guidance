using Godot;
using System;

public class Splash : Node2D
{
    public override void _Ready()
    {
        Position = GetViewportRect().Size / 2;
    }
}
