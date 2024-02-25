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
	
	public override void _Ready()
	{
		base._Ready();
		Focus = false;
		MouseOn = false;
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
