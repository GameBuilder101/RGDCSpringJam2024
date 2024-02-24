using Godot;
using System;

enum TitleState
{
	Title,
	Instructions
}
public partial class TitleScreen : Node2D
{
	[Export] double framesPerSecond;
	[Export] Texture2D[] frames;
	[Export] double flashInterval;
	[Export] Color[] flashColors;
	[Export] Sprite2D sprite;
	[Export] Label startLabel;
	[Export] Node instructions;
	[Export] Label instructionsLabel;
	[Export] ColorRect underline;

	double secondsPerFrame;
	double timer;
	int frameIndex;
	double flashTimer;
	int flashIndex;
	int instructionIndex;

	TitleState state;
	bool prevPressState;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		secondsPerFrame = 1 / framesPerSecond;
		timer = 0;
		frameIndex = 0;
		flashTimer = 0;
		flashIndex = flashColors.Length - 1;
		instructionIndex = 0;
		sprite.Texture = frames[frameIndex];
		startLabel.Visible = false;
		foreach (Node panel in instructions.GetChildren())
			((Sprite2D)panel).Visible = false;
        instructionsLabel.Visible = false;
        underline.Visible = false;

        state = TitleState.Title;
		prevPressState = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch(state)
		{
			case TitleState.Title:
                timer += delta;
                if (frameIndex < frames.Length - 1)
                {
					if (Mathf.Floor(timer / secondsPerFrame) > frameIndex)
					{
                        if (Mathf.Floor(timer / secondsPerFrame) > frames.Length - 1)
                            frameIndex = frames.Length - 1;
                        else
                            frameIndex = Mathf.FloorToInt(timer / secondsPerFrame);
                        sprite.Texture = frames[frameIndex];
                    }

					if (Input.IsAnythingPressed())
					{
						frameIndex = frames.Length - 1;
                        sprite.Texture = frames[frameIndex];
                    }
                }
                else
                {
                    flashTimer += delta;
                    while (flashTimer > flashInterval)
                    {
                        flashTimer -= flashInterval;
                        if (!startLabel.Visible)
                        {
                            flashIndex++;
                            if (flashIndex >= flashColors.Length)
                                flashIndex -= flashColors.Length;
                            SetLabelColor(flashIndex);
                        }
                        startLabel.Visible = !startLabel.Visible;
                    }

                    if (!prevPressState && Input.IsAnythingPressed())
                    {
                        state = TitleState.Instructions;
                        sprite.Visible = false;
                        startLabel.Visible = false;
                        ((Sprite2D)instructions.GetChild(instructionIndex)).Visible = true;
                        instructionsLabel.Visible = true;
                        underline.Visible = true;
                    }
                }

				prevPressState = Input.IsAnythingPressed();
				break;
			case TitleState.Instructions:
				int newIndex = instructionIndex;
				if (instructionIndex > 0 && Input.IsActionJustPressed("PrevPage"))
					newIndex--;
				if (Input.IsActionJustPressed("Go"))
				{
					if (instructionIndex == instructions.GetChildren().Count - 1)
						GetTree().ChangeSceneToFile("res://casino/Casino.tscn");
					else
                        newIndex++;
                }
				else if (instructionIndex < instructions.GetChildren().Count - 1 && Input.IsActionJustPressed("NextPage"))
					newIndex++;
				if (newIndex != instructionIndex)
				{
					((Sprite2D)instructions.GetChild(instructionIndex)).Visible = false;
                    ((Sprite2D)instructions.GetChild(newIndex)).Visible = true;
					instructionIndex = newIndex;
                }
				break;
        }
	}

	void SetLabelColor(int index)
	{
        startLabel.AddThemeColorOverride("font_color", flashColors[index]);
    }
}
