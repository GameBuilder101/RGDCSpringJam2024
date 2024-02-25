using Godot;
using System;

public partial class GameOver : Node2D
{
	private static double _timePlayed;
	private static int _highestMoney;
	private static int _timesFined;

	[Export]
	private Label _timePlayedLabel;
	[Export]
	private Label _highestMoneyLabel;
	[Export]
	private Label _timesFinedLabel;
	[Export]
	private Label _finalScoreLabel;

	private double _animTime;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_timePlayedLabel.Text = "Minutes in Business: " + Math.Round(_timePlayed / 60.0);
		_highestMoneyLabel.Text = "Most Money at Once: " + _highestMoney;
		_timesFinedLabel.Text = "Times Fined: " + _timesFined;
		_finalScoreLabel.Text = "" + CalculateFinalScore(_timePlayed, _highestMoney, _timesFined);
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		_animTime += delta;
		_finalScoreLabel.RotationDegrees = (float)(Math.Sin(_animTime * 2.0) - 0.5) * 5.0f;
    }

    public void SwitchToTitle()
    {
        GetTree().ChangeSceneToFile("res://title_screen/title_screen.tscn");
    }

    /// <summary>
    /// Call to set the values displayed on the game over screen.
    /// </summary>
    public static void SetScoreValues(double timePlayed, int highestMoney, int timesFined)
	{
		_timePlayed = timePlayed;
		_highestMoney = highestMoney;
		_timesFined = timesFined;
	}

	public static int CalculateFinalScore(double timePlayed, int highestMoney, int timesFined)
	{
		return ((int)timePlayed - timesFined * 4) * 10 + highestMoney;
	}
}
