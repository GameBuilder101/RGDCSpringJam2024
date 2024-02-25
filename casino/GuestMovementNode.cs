using Godot;
using System;

public partial class GuestMovementNode : Node2D
{
	/// <summary>
	/// The set of possible connected nodes
	/// </summary>
	[Export]
	private Node2D[] _connected;
	public Node2D[] Connected { get { return _connected; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
