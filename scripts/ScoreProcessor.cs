using Godot;
using System;

public partial class ScoreProcessor : Node
{
	long score = 0;
	int combo = 0;

	Label scoreLabel;
	Label comboLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		scoreLabel = GetNode<Label>("../UI/Score");
		comboLabel = GetNode<Label>("../UI/Combo");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void addScore(int scoreIncrease)
	{
		combo += 1;
		score += scoreIncrease * combo;

		updateScoreDisplay();
	}

	private void updateScoreDisplay()
	{
		scoreLabel.Text = score.ToString();
		comboLabel.Text = combo.ToString();
	}

	public void resetCombo()
	{
		combo = 0;
		updateScoreDisplay();
	}

}
