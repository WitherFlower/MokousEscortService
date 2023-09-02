using Godot;
using System;

public partial class Enemy : Area2D
{

	[Export]
	public PackedScene BulletScene { get; set; }

	public PathFollow2D pathFollow2D { get; set; }

	private int hp = 4;
	private double shotInterval = 2;
	private double shotTimer = 0;

	private const float maxLifeTime = 5;
	private float lifeTime = 0;

	public Path path;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		updatePosition();
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
		hp--;

		if (hp <= 0)
		{
			QueueFree();
		}
	}

}
