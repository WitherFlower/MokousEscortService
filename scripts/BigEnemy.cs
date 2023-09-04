using Godot;
using System;

public partial class BigEnemy : Area2D, IEnemy
{
	[Export]
	public PackedScene BulletScene { get; set; }

	[Export]
	public PackedScene ItemScene { get; set; }

	public PathFollow2D pathFollow2D { get; set; }

	private int hp = 4;
	private const double shotInterval = 0.6;
	private double shotTimer = 0;

	private const double shotWaveInterval = 0.1;
	private double shotWaveTimer = 0;
	private double shotWaveRemainingCount = 0;

	private float maxLifeTime = 5;
	private float lifeTime = 0;

	public Path path;

	ScoreProcessor scoreProcessor;
	public int scoreIncrease = 10;

	private AnimatedSprite2D animatedSprite2D;
	private Player player;

	private Random rand = new Random();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		scoreProcessor = GetNode<ScoreProcessor>("/root/Main/ScoreProcessor");
		player = GetNode<Player>("../Player");

		updatePosition();
		maxLifeTime = path.getLength() / 120f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		animatedSprite2D.Play("idle");

		lifeTime += (float)delta;
		updatePosition();

		shotTimer += delta;
		while (shotTimer > shotInterval)
		{
			shotTimer -= shotInterval;

			if (rand.Next() % 2 == 0)
			{
				fireBulletBarrage(10);
			}
			else
			{
				shotWaveRemainingCount = 3;
				shotWaveTimer = shotWaveInterval;
			}
		}

		if (shotWaveRemainingCount > 0)
		{
			shotWaveTimer += delta;
			while (shotWaveTimer > shotWaveInterval)
			{
				shotWaveTimer -= shotWaveInterval;
				shotWaveRemainingCount--;
				fireAimedBullet();
			}
		}

		if (lifeTime >= maxLifeTime)
		{
			QueueFree();
		}
	}

	private void fireAimedBullet()
	{
		Vector2 bulletDirection = (player.Position - Position).Normalized();

		Bullet bullet = BulletScene.Instantiate<Bullet>();
		bullet.Position = Position + bulletDirection * 4;
		bullet.velocity = bulletDirection * 100;

		AddSibling(bullet);
	}

	private void fireBulletBarrage(int bulletCount)
	{
		for (int i = 0; i < bulletCount; i++)
		{
			Vector2 bulletDirection = new Vector2(
				Mathf.Cos(2 * i * Mathf.Pi / bulletCount),
				Mathf.Sin(2 * i * Mathf.Pi / bulletCount)
			);

			Bullet bullet = BulletScene.Instantiate<Bullet>();
			bullet.Position = Position + bulletDirection * 4;
			bullet.velocity = bulletDirection * 100;

			AddSibling(bullet);
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

	public Area2D hitArea()
	{
		return this;
	}

	public void setPath(Path path)
	{
		this.path = path;
	}

	public void setMaxHealth(int enemyMaxHealth)
	{
		hp = enemyMaxHealth;
	}

}
