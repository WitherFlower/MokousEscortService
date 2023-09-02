using Godot;
using System;

public partial class Princess : Area2D
{

	const int speed = 300;
	Vector2 targetPos;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 movement = targetPos - Position;
		if (speed * delta > movement.Length())
		{
			Position = targetPos;
		}
		else
		{
			Position += movement.Normalized() * speed * (float)delta;
		}
	}

	public void SetTargetPos(Vector2 targetPos)
	{
		this.targetPos = targetPos;
	}

}
