using Godot;
using System;

public partial class Enemy : Area2D
{

	[Export]
	public PackedScene BulletScene { get; set; }

	private int hp = 4;
	private double shotInterval = 2;
	private double shotTimer = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		shotTimer += delta;
		while (shotTimer > shotInterval)
		{
			shotTimer -= shotInterval;
			for (int i = 0; i < 16; i++)
			{
				Vector2 bulletDirection = new Vector2(
					Mathf.Cos(2 * i * Mathf.Pi / 16),
					Mathf.Sin(2 * i * Mathf.Pi / 16)
				);

				Bullet bullet = BulletScene.Instantiate<Bullet>();
				bullet.Position = Position + bulletDirection * 4;
				bullet.velocity = bulletDirection * 100;
				AddSibling(bullet);
			}
		}
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
