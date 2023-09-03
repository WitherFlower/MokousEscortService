using Godot;
using System;

public partial class Enemy : Area2D
{

	[Export]
	public PackedScene BulletScene { get; set; }

	[Export]
	public PackedScene ItemScene { get; set; }

	public PathFollow2D pathFollow2D { get; set; }

	private int hp = 4;
	private double shotInterval = 2;
	private double shotTimer = 0;

	private const float maxLifeTime = 5;
	private float lifeTime = 0;

	public Path path;

	ScoreProcessor scoreProcessor;
	public int scoreIncrease = 10;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		updatePosition();
		scoreProcessor = GetNode<ScoreProcessor>("/root/Main/ScoreProcessor");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		lifeTime += (float)delta;
		updatePosition();

		shotTimer += delta;
		while (shotTimer > shotInterval)
		{
			shotTimer -= shotInterval;
			for (int i = 0; i < 8; i++)
			{
				Vector2 bulletDirection = new Vector2(
					Mathf.Cos(2 * i * Mathf.Pi / 8),
					Mathf.Sin(2 * i * Mathf.Pi / 8)
				);

				Bullet bullet = BulletScene.Instantiate<Bullet>();
				bullet.Position = Position + bulletDirection * 4;
				bullet.velocity = bulletDirection * 100;

				AddSibling(bullet);
			}
		}

		if (lifeTime >= maxLifeTime)
		{
			QueueFree();
		}
	}

	private void updatePosition()
	{
		if (path == null)
		{
			return;
		}
		Position = path.getPosition(lifeTime / maxLifeTime);
	}

	public void onHit()
	{
		// easy fix to prevent killing enemies before they appear on screen
		if (lifeTime > 0.5)
		{
			hp--;
		}

		if (hp <= 0)
			kill();
	}

	public void kill()
	{
		var item = ItemScene.Instantiate<PowerItem>();
		item.Position = Position;
		AddSibling(item);

		scoreProcessor.addScore(scoreIncrease);

		QueueFree();
	}

}
