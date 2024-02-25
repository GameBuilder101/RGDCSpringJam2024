using Godot;
using System;

public partial class CasinoBackground : Sprite2D
{
	[Export]
	private Texture2D[] _textures;
	private int _currentTexture;

	[Export]
	private double _switchTime;
	private double _currentTimer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        UpdateToCurrentTexture();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_currentTimer += delta;
		if (_currentTimer >= _switchTime)
		{
			_currentTimer = 0.0;
			_currentTexture++;
			if (_currentTexture >= _textures.Length)
				_currentTexture = 0;
			UpdateToCurrentTexture();

        }
	}

	private void UpdateToCurrentTexture()
	{
		Texture = _textures[_currentTexture];
	}
}
