using Godot;
using System;

public partial class LocationControl : Control
{
	[Export]
	private Sprite2D Diamond;
	
	public int index;
	
	public override void _Ready()
    {
        base._Ready();
		Diamond.Visible = false;
    }
	
	private void OnMouseEntered()
	{
	    Diamond.Visible = true;
	}

	private void OnMouseExited()
	{
	    Diamond.Visible = false;
	}
	
	private void OnPressed()
	{
	    // Replace with function body.
	}
}
