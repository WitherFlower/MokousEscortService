using Godot;
using System;

public partial class player : Area2D
{

	[Export]
	public int Speed { get; set; } = 300;

	[Export]
	public PackedScene ProjectileScene { get; set; }

	Vector2 PlayfieldSize;
	Vector2 PlayerSize = new Vector2(32, 48);

	Princess Princess;
	const int princessDistance = 32;

	const float shotInterval = 1 / 20f;
	float shotTimer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayfieldSize = (Vector2)GetNode("..").Get("TextureSize");
		Princess = (Princess)GetNode("Princess");
		Princess.SetTargetPos(new Vector2(0, princessDistance));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			velocity.Y -= 1;
		}

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;

			if (Input.IsActionPressed("focus"))
			{
				velocity *= 0.5f;
			}
			else
			{
				Princess.SetTargetPos(-velocity.Normalized() * princessDistance);
			}
		}

		Position += velocity * (float)delta;
		Position = new Vector2(
			Mathf.Clamp(Position.X, PlayerSize.X / 2, PlayfieldSize.X - PlayerSize.X / 2),
			Mathf.Clamp(Position.Y, PlayerSize.Y / 2, PlayfieldSize.Y - PlayerSize.Y / 2)
		);

		if (Input.IsActionPressed("fire"))
		{
			shotTimer += (float)delta;

			while (shotTimer > shotInterval)
			{
				shotTimer -= shotInterval;

				var projectile = ProjectileScene.Instantiate<Projectile>();
				projectile.Position = Position + new Vector2(0, -30) + projectile.Speed * shotTimer;

				AddSibling(projectile);
			}
		}
	}
}
