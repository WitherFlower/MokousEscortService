using Godot;
using System;

public partial class Enemy : Area2D, IEnemy
{

	[Export]
	public PackedScene ItemScene { get; set; }

	public PathFollow2D pathFollow2D { get; set; }

	private int hp = 4;

	private float maxLifeTime = 5;
	private float lifeTime = 0;

	public Path path;

	ScoreProcessor scoreProcessor;
	public int scoreIncrease = 10;

	private AnimatedSprite2D animatedSprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		scoreProcessor = GetNode<ScoreProcessor>("/root/Main/ScoreProcessor");

		updatePosition();
		maxLifeTime = path.getLength() / 140f;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		animatedSprite2D.Play("idle");

		lifeTime += (float)delta;
		updatePosition();

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
