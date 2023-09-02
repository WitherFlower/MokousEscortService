using Godot;
using System;

public partial class Bullet : Area2D
{

	public Vector2 velocity { get; set; }

	Node playfield;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playfield = GetNode("..");
	}
	// Called when the node enters the scene tree for the first time.

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += velocity * (float)delta;
	}

	private void _on_area_exited(Area2D area)
	{
		if (area == playfield)
		{
			QueueFree();
		}
	}
}
