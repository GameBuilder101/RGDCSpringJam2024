using Godot;
using System;

public partial class BuyNew : Node2D
{
	public static BuyNew instance {get; private set;}
	
	private int MachineViewIndex = 0;
	
	[Export]
	private Machine[] MachineTypes;
	
	[Export]
	private Label ShopCostLabel;
	[Export]
	private Sprite2D MachineImage;
	[Export]
	private Label NameLabel;
	[Export]
	private Label CostLabel;
	[Export]
	private Label JackpotLabel;
	[Export]
	private Label SusLabel;
	[Export]
	private Label RollsLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		UpdateDisplay();
	}
	
	public void UpdateDisplay() {
		Machine m = MachineTypes[MachineViewIndex];
		ShopCostLabel.Text = string.Format("Buy ${0}", m.ShopCost);
		MachineImage.Texture = m.MachineTexture;
		NameLabel.Text = m.name;
		CostLabel.Text = string.Format("Payout Per Play: ${0}", m.PlayCost);
		JackpotLabel.Text = string.Format("Jackpot Cost: ${0}", m.JackpotAmount);
		SusLabel.Text = string.Format("Suspicion Factor: {0:00}%", m.DefaultSuspicionFactor * 100);
		RollsLabel.Text = string.Format("Rolls Per Guest: {0}", m.NumRolls);
	}
	
	public void ChangeMachine(int change) {
		MachineViewIndex = (
			MachineViewIndex + change + MachineTypes.Length
				) % MachineTypes.Length;
		UpdateDisplay();
	}
	
	public void Buy() {
		Machine m = MachineTypes[MachineViewIndex];
		if (MachineManager.instance.Moola < m.ShopCost) {
			return;
		}
		MachineManager.instance.placeMachine(m.copy());
		MachineManager.instance.Moola -= m.ShopCost;
		Machine display = MachineManager.instance.getMachine();

		Visible = false;
		MachineRiggingMenu.Instance.Visible = true;
		MachineRiggingMenu.Instance.OpenOnMachine(display);
	}
}
