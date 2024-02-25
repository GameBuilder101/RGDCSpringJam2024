using Godot;
using System;

public partial class LocationControl : Control
{
	[Export]
	private Sprite2D Diamond;
	
	public int index;
	private bool _f;
	public bool Focus {get {return _f;} set {_f = value; updateDiamond();}}
	private bool _m;
	private bool MouseOn {get {return _m;} set {_m = value; updateDiamond();}}

	private double _animTime;
	
	public override void _Ready()
	{
		base._Ready();
		Focus = false;
		MouseOn = false;
	}

    public override void _Process(double delta)
    {
        base._Process(delta);
		_animTime += delta;
		float scale = 1.0f + (float)Math.Sin(_animTime * 3.0) * 0.1f;

        Diamond.Scale = new Vector2(scale, scale);
    }

    private void OnMouseEntered()
	{
		MouseOn = true;
	}

	private void OnMouseExited()
	{
		MouseOn = false;
	}
	
	private void OnPressed()
	{
		MachineShop.instance.ShowSlot(index);
	}
	
	private void updateDiamond() {
		Diamond.Visible = Focus || MouseOn;
	}
}
