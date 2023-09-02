using Godot;
using System;

public partial class Projectile : Area2D
{

	private Vector2 Speed = new Vector2(0, -800);

	Node playfield;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playfield = GetNode("..");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Speed * (float)delta;
		var enemies = GetTree().GetNodesInGroup("enemies");

		foreach (Enemy enemy in enemies)
		{

			if (OverlapsArea(enemy))
			{
				enemy.onHit();
				QueueFree();
			}

		}
	}

	private void _on_area_exited(Area2D area)
	{
		if (area == playfield)
		{
			QueueFree();
		}
	}
}
