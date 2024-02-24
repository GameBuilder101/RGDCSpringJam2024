using Godot;
using System;

public partial class BuyButton : TextureButton
{
	private void OnPressed()
	{
	    BuyNew.instance.Buy();
	}
}
