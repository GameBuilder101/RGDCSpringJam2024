using Godot;
using System;

public partial class MachineShop : Node2D
{
	public static MachineShop instance;
	private int ViewIndex;
	
	[Export]
	private Node2D BuyNew;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		Visible = false;
	}
	
	public void ShowSlot(int index) {
		ViewIndex = index;
		MachineManager.instance.focus(index);
		Machine display = MachineManager.instance.getMachine(index);
		if (display == null) {
			BuyNew.Visible = true;
		}
		Visible = true;
	}
	
	public void HideShop() {
		base.Hide();
		MachineManager.instance.focus(-1);
	}
}
