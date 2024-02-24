using Godot;
using System;

public partial class MachineManager : Node2D
{
	public static MachineManager instance;
	
	private double Suspicion = 0.0;
	private int Moola = 0;
	private double Progress = 0.0;
	private double TimeBetweenTicks = 5.0;
	private Machine[] Machines;
	
	[Export]
	private Node2D[] Locations;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		Machines = new Machine[Locations.Length];
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
				m.Tick(this);
			}
		}
		if (Moola < 0) {
			// switch to end screen
		}
		if (Suspicion > 0.9) {
			// call the inspector
		}
	}
	
	public void placeMachine(int index, Machine m) {
		m.Position = Locations[index].Position;
		Machines[index] = m;
		AddChild(m);
	}
	
	public void removeMachine(int index) {
		Machines[index].QueueFree();
		Machines[index] = null;
	}
}