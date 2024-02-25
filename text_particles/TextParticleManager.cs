using Godot;
using System;

public partial class TextParticleManager : Node
{
	[Export]
	private Color moneyIncrease;
	[Export]
	private Color moneyDecrease;
	[Export]
	private Color fineColor;
	[Export]
	private Node2D fineSpawn;
	
	public static TextParticleManager instance {get; private set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
	}

	public void createMoneyChangeParticle(string msg, int change, Vector2 at) {
		Color color;
		string prepend;
		if (change < 0) {
			color = moneyDecrease;
			prepend = "";
		} else {
			color = moneyIncrease;
			prepend = "+";
		}
		if (msg != "") {
			prepend = "\n" + prepend;
		}
		AddChild(new TextParticle(
			msg + prepend + change,
			at, color, 0.5 * Math.Log(Math.Abs(change)), 10
		));
	}
	
	public void createFineMessage(int numFine, long fineAmount) {
		string suffix;
		if (10 < numFine % 100 && numFine % 100 < 20) {
			suffix = "th";
		} else {
			switch (numFine % 10) {
				case 1:
					suffix = "st";
					break;
				case 2:
					suffix = "nd";
					break;
				case 3:
					suffix = "rd";
					break;
				default:
					suffix = "th";
					break;
			}
		}
		AddChild(new TextParticle(
			numFine + suffix + " fine!\n" + fineAmount,
			fineSpawn.Position,
			fineColor, 0.5 * Math.Log(Math.Abs(fineAmount)),
			20
		));
	}
}
