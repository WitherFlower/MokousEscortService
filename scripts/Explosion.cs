using Godot;
using System;

public partial class Explosion : Node2D
{

	private const double explosionDuration = 1.2;
	private double explosionTimer = 0;
	Player player;
	bool explosionStarted = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<Player>("../Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (player.exploding)
		{
			if (!explosionStarted)
			{
				explosionTimer = 0;
				explosionStarted = true;
			}

			explosionTimer += delta;

			if (explosionTimer > explosionDuration)
			{
				player.exploding = false;
				explosionStarted = false;
			}

			QueueRedraw();
		}
	}

	public override void _Draw()
	{
		if (!player.exploding)
		{
			return;
		}

		var explosionProgress = (float)explosionTimer / (float)explosionDuration;
		var alpha = Mathf.Max(0, Mathf.Min(1, -2 * explosionProgress + 2));
		var circleColor = new Color(1, 1, explosionProgress, alpha);

		DrawCircle(player.Position, explosionProgress * 500, circleColor);
	}
}
