using Godot;
using System;

public partial class EventPlayableAudio : AudioStreamPlayer2D
{
	public void Play()
	{
		if (Playing)
			Stop();
		if (IsInsideTree())
			Playing = true;
	}
}
