using Godot;
using System;

public partial class LoopAudio : AudioStreamPlayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Playing = true;
	}
	
	private void Finished()
	{
	    Playing = true;
	}
}
