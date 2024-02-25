using Godot;
using System;

public partial class UIMeter : Node2D
{
	[Export]
	private Sprite2D _meterFill;
	[Export]
	private Label _moneyLabel;

	float _origSizeX;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_origSizeX = _meterFill.RegionRect.Size.X;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Get region size
		Rect2 region = _meterFill.RegionRect;
		// Modify region size
		region.Size = new Vector2((float)MachineManager.instance.Suspicion * _origSizeX, region.Size.Y);
		// Set region size
        _meterFill.RegionRect = region;

        _moneyLabel.Text = "$" + MachineManager.instance.Moola;
	}
}
