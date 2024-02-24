using Godot;
using System;

public partial class TitleScreen : Node2D
{
	[Export] double framesPerSecond;
	[Export] Texture2D[] frames;
	[Export] Sprite2D sprite;

	double secondsPerFrame;
	double timer;
	int frameIndex;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		secondsPerFrame = 1 / framesPerSecond;
		timer = 0;
		frameIndex = 0;
		sprite.Texture = frames[frameIndex];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer += delta;
		if (frameIndex < frames.Length - 1 && Mathf.Floor(timer / secondsPerFrame) > frameIndex)
		{
			if (Mathf.Floor(timer / secondsPerFrame) > frames.Length - 1)
				frameIndex = frames.Length - 1;
			else
				frameIndex = Mathf.FloorToInt(timer / secondsPerFrame);
			sprite.Texture = frames[frameIndex];
        }
	}
}
