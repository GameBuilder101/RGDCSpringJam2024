using Godot;
using System;

public partial class CancelButton : TextureButton
{
	private void OnPressed()
	{
	    MachineShop.instance.HideShop();
	}
}
