using Godot;
using System;

public partial class PowerItem : Area2D
{

	public Vector2 acceleration = new Vector2(0, 200);
	private Vector2 speed = new Vector2(0, -150);

	Node playfield;
	Player player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playfield = GetNode("..");
		player = GetNode<Player>("../Player");
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		speed += acceleration / 2 * (float)delta;
		Position += speed * (float)delta;
		speed += acceleration / 2 * (float)delta;

		if (OverlapsArea(player))
		{
			QueueFree();
		}
	}

	private void _on_area_exited(Area2D area)
	{
		// don't destroy if playfield is exited at the top
		if (area == playfield && Position.Y > 0)
		{
			QueueFree();
		}
	}
}
