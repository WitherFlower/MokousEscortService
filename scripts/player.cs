using Godot;
using System;

public partial class Player : Area2D
{

	public int Speed { get; set; } = 300;

	[Export]
	public PackedScene ProjectileScene { get; set; }

	Vector2 PlayfieldSize;
	Vector2 PlayerSize = new Vector2(32, 48);

	Princess Princess;
	const int princessDistance = 24;

	const float shotInterval = 1 / 15f;
	float shotTimer = 0;

	const float stunDelay = 1;
	float stunTimer = 0;

	int pendingPower = 0;
	// int bankedPower = 0;

	private AnimatedSprite2D animatedSprite2D;
	ScoreProcessor scoreProcessor;
	internal bool exploding = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		PlayfieldSize = (Vector2)GetNode("..").Get("TextureSize");

		Princess = (Princess)GetNode("Princess");
		Princess.SetTargetPos(new Vector2(0, princessDistance));

		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		scoreProcessor = GetNode<ScoreProcessor>("/root/Main/ScoreProcessor");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (stunTimer <= 0)
		{
			updatePosition(delta);
		}
		else
		{
			stunTimer -= (float)delta;
		}

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

	private void updatePosition(double delta)
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

		animatedSprite2D.Play("idle");

		Position += velocity * (float)delta;
		Position = new Vector2(
			Mathf.Clamp(Position.X, PlayerSize.X / 2, PlayfieldSize.X - PlayerSize.X / 2),
			Mathf.Clamp(Position.Y, PlayerSize.Y / 2, PlayfieldSize.Y - PlayerSize.Y / 2)
		);

		if (velocity.X < 0)
		{
			animatedSprite2D.Play("left");
		}
		else if (velocity.X > 0)
		{
			animatedSprite2D.Play("right");
		}
	}

	public void onHit()
	{
		if (stunTimer <= 0)
			stunTimer = stunDelay;

		if (pendingPower < 20)
			return;

		// Deathbomb
		var enemies = GetTree().GetNodesInGroup("enemies");
		foreach (IEnemy enemy in enemies)
		{
			enemy.kill();
		}

		var bullets = GetTree().GetNodesInGroup("bullets");
		foreach (Bullet bullet in bullets)
		{
			bullet.QueueFree();
		}

		// bankedPower += pendingPower;
		pendingPower = 0;

		exploding = true;
	}

	internal void addPower()
	{
		pendingPower++;
	}

	internal void onPrincessHit()
	{
		scoreProcessor.resetCombo();
	}

}
