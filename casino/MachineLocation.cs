using Godot;
using System;

public partial class MachineLocation : Node2D
{
	[Export]
	private LocationControl loc;
	
	public void setIndex(int index) {
		loc.index = index;
	}
}
