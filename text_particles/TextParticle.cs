using Godot;
using System;

public partial class TextParticle : Label
{
	private static Color outlineColor = new Color(0, 0, 0);

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
		Set("theme_override_colors/font_outline_color", outlineColor);
		Set("theme_override_constants/outline_size", 2);
		Set("theme_override_font_sizes/font_size", 10);
		this.HorizontalAlignment = (HorizontalAlignment) 1;
		this.LifeTime = lifeTime;
		this.TotalLifeTime = lifeTime;
		this.ZIndex = 1;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		this.LifeTime -= delta;
		this.GlobalPosition += new Vector2(0.0f, -1.1f * (float)delta);

		if (this.LifeTime <= 0) {
			this.QueueFree();
		} else {
			if (this.LifeTime / this.TotalLifeTime > 0.5) {
				return;
			}
			this.textColor.A = (float) Math.Sqrt(2 * this.LifeTime / this.TotalLifeTime);
			Set("theme_override_colors/font_color", textColor);
		}
	}
}
