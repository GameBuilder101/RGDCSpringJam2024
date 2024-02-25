using Godot;
using System;

public partial class TextParticle : Label
{

	private static Vector2 movement = new Vector2(0, -5);

	private Color textColor;
	private double LifeTime;
	private double TotalLifeTime;

	public TextParticle() {}

	public TextParticle(
		string text,
		Vector2 position,
		Color color,
		double lifeTime
	) {
		this.Text = text;
		this.Position = position;
		this.textColor = new Color(color);
		Set("theme_override_colors/font_color", textColor);
		this.LifeTime = lifeTime;
		this.TotalLifeTime = lifeTime;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.LifeTime -= delta;
		this.Position += movement * (float) delta;
		if (this.LifeTime <= 0) {
			this.QueueFree();
		} else {
			this.textColor.A = (float) Math.Sqrt(this.LifeTime / this.TotalLifeTime);
		}
	}
}
