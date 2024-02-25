using Godot;
using System;

public partial class FadeOverlay : ColorRect
{
	private float opaque {
		get {return Color.A;} set {
			Color = new Color(0, 0, 0, Math.Max(Math.Min(value, 1), 0));
			Visible = Color.A != 0;
		}
	}
	[Export]
	private float speed = 0.5f;
	private int direction;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		opaque = 1;
		direction = -1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		opaque += (float) (delta * direction * speed);
		if (direction > 0 && opaque == 1) {
			GetTree().ChangeSceneToFile("res://game_over/game_over.tscn");
		}
	}
	
	public void fadeOut() {
		direction = 1;
	}
}
