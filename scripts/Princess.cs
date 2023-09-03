using Godot;
using System;

public partial class Princess : Area2D
{

	const int speed = 300;
	Vector2 targetPos;

	private AnimatedSprite2D animatedSprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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

		animatedSprite2D.Play("idle");

		if (movement.X < 0)
		{
			animatedSprite2D.Play("left");
		}
		else if (movement.X > 0)
		{
			animatedSprite2D.Play("right");
		}
	}

	public void SetTargetPos(Vector2 targetPos)
	{
		this.targetPos = targetPos;
	}

}
