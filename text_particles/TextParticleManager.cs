using Godot;
using System;

public partial class TextParticleManager : Node
{
	[Export]
	private Color moneyIncrease;
	[Export]
	private Color moneyDecrease;
	
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
		AddChild(new TextParticle(
			msg + "\n" + prepend + change, at, color, Math.Sqrt(Math.Abs(change))
		));
	}
}
