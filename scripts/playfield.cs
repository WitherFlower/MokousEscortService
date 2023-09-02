using Godot;
using System;

public partial class playfield : Area2D
{

	[Export]
	public PackedScene EnemyScene { get; set; }

	const double enemySpawnInterval = 0.5;
	private double enemySpawnTimer = 0;

	[Export]
	public Vector2 TextureSize { get; set; } = new Vector2(256, 320);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		enemySpawnTimer += delta;
		while (enemySpawnTimer >= enemySpawnInterval)
		{
			enemySpawnTimer -= enemySpawnInterval;

			var enemy = EnemyScene.Instantiate<Enemy>();
			enemy.Position = new Vector2(
				(GD.Randi() % 224) + 16,
				50
			);
			AddChild(enemy);
			GD.Print("spawning new enemy");
		}
	}
}
