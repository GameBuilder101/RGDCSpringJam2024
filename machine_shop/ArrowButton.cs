using Godot;
using System;

public partial class ArrowButton : TextureButton
{
	private void OnPressed(int change)
	{
	    BuyNew.instance.ChangeMachine(change);
	}
	
	private void LeftPressed()
	{
	    OnPressed(-1);
	}
	
	private void RightPressed()
	{
	    OnPressed(1);
	}
}
