using Godot;
using System;

public partial class MachineLocation : Node2D
{
	[Export]
	private LocationControl loc;
	
	public bool Focus {get {return loc.Focus;} set {loc.Focus = value;}}
	
	public void setIndex(int index) {
		loc.index = index;
	}
}
