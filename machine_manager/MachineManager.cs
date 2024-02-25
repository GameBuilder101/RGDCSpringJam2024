using Godot;
using System;

public partial class MachineManager : Node2D
{
	public static MachineManager instance;

	public double Suspicion { get; set; } = 0.0;
	public int Moola { get; set; } = 10;
	private double Progress = 0.0;
	private double TimeBetweenTicks = 5.0;
	private Machine[] Machines;
	private MachineLocation[] Locations;
	public int FocussedLocation {get; private set;} = -1;
	
	[Export]
	private Node LocationHolder;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		int num = LocationHolder.GetChildCount();
		Locations = new MachineLocation[num];
		int index = 0;
		MachineLocation loc;
		foreach(Node l in LocationHolder.GetChildren()) {
			loc = (MachineLocation) l;
			Locations[index] = loc;
			loc.setIndex(index);
			index += 1;
		}
		Machines = new Machine[num];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Progress += delta;
		if (Progress >= TimeBetweenTicks) {
			Progress -= TimeBetweenTicks;
			foreach (Machine m in Machines) {
				if (m == null) {
					continue;
				}
				m.Tick();
			}
			GD.Print(Moola);
			GD.Print(Suspicion);
		}
		if (Moola < 0) {
			// switch to end screen
		}
		if (Suspicion > 0.9) {
			// call the inspector
		}
	}
	
	public void placeMachine(Machine m) {
		placeMachine(FocussedLocation, m);
	}
	
	public void placeMachine(int index, Machine m) {
		m.Position = Locations[index].Position + new Vector2(0, -8);
		Machines[index] = m;
		AddChild(m);
	}
	
	public void removeMachine(int index) {
		Machines[index].QueueFree();
		Machines[index] = null;
	}
	
	public Machine getMachine(int index) {
		return Machines[index];
	}
	
	public Machine getMachine() {
		return getMachine(FocussedLocation);
	}
	
	// -1 to unfocus
	public void focus(int index) {
		if (FocussedLocation != -1) {
			Locations[FocussedLocation].Focus = false;
		}
		FocussedLocation = index;
		if (FocussedLocation != -1) {
			Locations[FocussedLocation].Focus = true;
		}
	}
}
