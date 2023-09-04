using Godot;
using System;
using System.Collections.Generic;

public partial class playfield : Area2D
{

	[Export]
	public PackedScene EnemyScene { get; set; }

	[Export]
	public PackedScene BigEnemyScene { get; set; }

	const double enemySpawnInterval = 2;
	private double enemySpawnTimer = 0;

	[Export]
	public Vector2 TextureSize { get; set; } = new Vector2(256, 320);

	private List<Path> enemyPaths;
	private Random rand = new Random();

	int currentWaveRemainingEnemies = 0;
	const double waveSpawnInterval = 0.25;
	private double waveSpawnTimer = 0;
	Path currentWavePath;

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
			SpawnNewEnemyWave();
		}

		if (currentWaveRemainingEnemies > 0)
		{
			waveSpawnTimer += delta;
			while (waveSpawnTimer >= waveSpawnInterval)
			{
				currentWaveRemainingEnemies--;
				waveSpawnTimer -= waveSpawnInterval;
				SpawnNewEnemy<Enemy>(EnemyScene, 3);
			}
		}
	}


	private void SpawnNewEnemyWave()
	{
		currentWavePath = enemyPaths[rand.Next() % enemyPaths.Count];

		bool bigFairy = rand.Next() % 3 == 0;

		if (bigFairy)
		{
			SpawnNewEnemy<BigEnemy>(BigEnemyScene, 12);
		}
		else
		{
			currentWaveRemainingEnemies = 5;
			waveSpawnTimer = waveSpawnInterval;
		}

	}

	private void SpawnNewEnemy<T>(
		PackedScene enemyScene, int enemyMaxHealth /*, int tierLevel = 1 */
	) where T : Node, IEnemy
	{
		var enemy = enemyScene.Instantiate<T>();

		enemy.setPath(currentWavePath);
		enemy.setMaxHealth(enemyMaxHealth);

		AddChild(enemy);
	}
}
