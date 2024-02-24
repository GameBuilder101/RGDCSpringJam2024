using Godot;
using System;

public partial class MachineShop : Node2D
{
	public static MachineShop instance;
	private int ViewIndex;
	
	[Export]
	private BuyNew buyNew;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		Visible = false;
	}
	
	public void ShowSlot(int index) {
		// get slot data
		ViewIndex = index;
		// if clicked on same spot 2x, unfocus it and hide
		if (index == MachineManager.instance.FocussedLocation) {
			HideShop();
			return;
		}
		// focus on the correct slot on board
		MachineManager.instance.focus(index);
		Machine display = MachineManager.instance.getMachine(index);
		// if empty, show buy new screen
		buyNew.Visible = display == null;
		// if no errors, show
		Visible = true;
	}
	
	public void HideShop() {
		base.Hide();
		MachineManager.instance.focus(-1);
	}
}
