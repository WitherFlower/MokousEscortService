using Godot;
using System;
using System.Collections.Generic;

public partial class playfield : Area2D
{

	[Export]
	public PackedScene EnemyScene { get; set; }

	const double enemySpawnInterval = 0.5;
	private double enemySpawnTimer = 0;

	[Export]
	public Vector2 TextureSize { get; set; } = new Vector2(256, 320);

	private List<Path> enemyPaths;
	private Random rand = new Random();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyPaths = new List<Path>
		{
			new LinearPath(new Vector2(-32, 50), new Vector2(288, 50)),
			new LinearPath(new Vector2(288, 100), new Vector2(-32, 100)),
			new CompositePath(
				new List<Path>{
					new LinearPath(new Vector2(-32, -32), new Vector2(224, 100)),
					new LinearPath(new Vector2(224, 100), new Vector2(-32, 232)),
				}
			),
			new CompositePath(
				new List<Path>{
					new LinearPath(new Vector2(288, -32), new Vector2(32, 100)),
					new LinearPath(new Vector2(32, 100), new Vector2(288, 232)),
				}
			),
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		enemySpawnTimer += delta;
		while (enemySpawnTimer >= enemySpawnInterval)
		{
			enemySpawnTimer -= enemySpawnInterval;
			SpawnNewEnemy();
		}
	}

	private void SpawnNewEnemy()
	{
		var path = enemyPaths[rand.Next() % enemyPaths.Count];

		var enemy = EnemyScene.Instantiate<Enemy>();
		enemy.path = path;
		// enemy.Position = new Vector2(
		// 	(GD.Randi() % 224) + 16,
		// 	50
		// );
		AddChild(enemy);
		GD.Print("spawning new enemy");
	}

}
