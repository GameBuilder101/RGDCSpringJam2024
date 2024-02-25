using Godot;
using System;

enum TitleState
{
	Credits,
	Title,
	Instructions
}
public partial class TitleScreen : Node2D
{
	[Export] double framesPerSecond;
	[Export] Label creditsText;
	[Export] double creditsDuration;
	[Export] Texture2D[] frames;
	[Export] double flashInterval;
	[Export] Color[] flashColors;
	[Export] Sprite2D sprite;
	[Export] Label startLabel;
	[Export] Node2D instructionsParent;
	[Export] Node2D[] instructions;

	[Export] EventPlayableAudio _startAudio;

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
        sprite.Visible = false;
        creditsText.Visible = true;
        startLabel.Visible = false;
		instructionsParent.Visible = false;
		/*
		foreach (Node panel in instructions.GetChildren())
			((Sprite2D)panel).Visible = false;*/

        state = TitleState.Credits;
		prevPressState = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch(state)
		{
			case TitleState.Credits:
				timer += delta;
				if (timer >= creditsDuration)
				{
					state = TitleState.Title;
                    creditsText.Visible = false;
					sprite.Visible = true;
					timer = 0.0;
                }
				break;
			case TitleState.Title:
                timer += delta;

                startLabel.RotationDegrees = (float)(Math.Sin(timer * 2.0) - 0.5) * 5.0f;

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
                        instructionsParent.Visible = true;
						//((Sprite2D)instructions.GetChild(instructionIndex)).Visible = true;

						_startAudio.Play();
                    }
                }

				prevPressState = Input.IsAnythingPressed();
				break;
			case TitleState.Instructions:
				/*int newIndex = instructionIndex;
				if (instructionIndex > 0 && Input.IsActionJustPressed("PrevPage"))
					newIndex--;
				if (Input.IsActionJustPressed("Go"))
				{
					if (instructionIndex == instructionsParent.GetChildren().Count - 1)
						GetTree().ChangeSceneToFile("res://casino/Casino.tscn");
					else
                        newIndex++;
                }
				else if (instructionIndex < instructionsParent.GetChildren().Count - 1 && Input.IsActionJustPressed("NextPage"))
					newIndex++;
				if (newIndex != instructionIndex)
				{
					((Sprite2D)instructionsParent.GetChild(instructionIndex)).Visible = false;
                    ((Sprite2D)instructionsParent.GetChild(newIndex)).Visible = true;
					instructionIndex = newIndex;
                }*/
				break;
        }
	}

	void SetLabelColor(int index)
	{
        startLabel.AddThemeColorOverride("font_color", flashColors[index]);
    }

    public void PreviousInstructionPage()
    {
        instructions[instructionIndex].Visible = false;
        instructionIndex--;
		if (instructionIndex < 0)
			instructionIndex = 0;
        instructions[instructionIndex].Visible = true;
    }

    public void NextInstructionPage()
	{
		instructions[instructionIndex].Visible = false;
		instructionIndex++;
		if (instructionIndex >= instructions.Length)
			EndInstructions();
		else
			instructions[instructionIndex].Visible = true;
    }

	public void EndInstructions()
	{
        GetTree().ChangeSceneToFile("res://casino/Casino.tscn");
    }
}
