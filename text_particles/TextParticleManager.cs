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
		if (change < 0) {
			color = moneyDecrease;
		} else {
			color = moneyIncrease;
		}
		AddChild(new TextParticle(
			msg + "\n" + change, at, color, ((double) change) / 128.0
		));
	}
}
