using Godot;
using System;

public partial class MachineRiggingMenu : Node2D
{
	public static MachineRiggingMenu Instance { get; private set; }

	private Machine _targetMachine;
	/// <summary>
	/// Gets or sets the location index of the target machine.
	/// </summary>
	public Machine TargetMachine
	{
		get { return _targetMachine; }
		set
		{
			_targetMachine = value;
		}
	}
	/// <summary>
	/// Gets the sell amount for the current target machine.
	/// </summary>
	private int SellCost { get { return TargetMachine.ShopCost / 2; } }

	[Export]
	private Label _machineNameLabel;
	[Export]
	private Sprite2D _machineSprite;
	[Export]
	private Label _playCostLabel;
	[Export]
	private Label _sellCostLabel;
	[Export]
	private Label _jackpotAmountLabel;
	[Export]
	private Label _suspicionFactorLabel;
	[Export]
	private Label _numRollsLabel;
	[Export]
	private Label _winChanceLabel;

	public override void _Ready()
	{
		base._Ready();
		Instance = this;

	}

	public void Sell()
	{
		MachineManager.instance.Moola += SellCost;
		MachineManager.instance.removeMachine(MachineShop.instance.ViewIndex);
		MachineShop.instance.HideShop();
	}

	public void OpenOnMachine(Machine machine)
	{
		TargetMachine = machine;

		_machineNameLabel.Text = machine.name;
		_machineSprite.Texture = machine.MachineTexture;
		_playCostLabel.Text = "Payout Per Play: $" + machine.PlayCost;
		_sellCostLabel.Text = "Sell $" + SellCost;
		UpdateDynamicDisplay();
		_numRollsLabel.Text = "Rolls Per Guest: " + machine.NumRolls;
	}

	private void UpdateDynamicDisplay() {
		_winChanceLabel.Text = Math.Round(TargetMachine.JackpotProbability * 100) + "%";
		_suspicionFactorLabel.Text = "Suspicion Factor: " + (Math.Round(TargetMachine.SuspicionFactor * 100000.0))/1000 + "%";
	}

	public void IncreaseWinChance()
	{
		TargetMachine.JackpotProbability += 0.01f;
		if (TargetMachine.JackpotProbability > 1.0f)
			TargetMachine.JackpotProbability = 1.0f;
		RoundWinChance();
		TargetMachine.SuspicionUpdater();
		UpdateDynamicDisplay();
	}

	public void DecreaseWinChance()
	{
		TargetMachine.JackpotProbability -= 0.01f;
		if (TargetMachine.JackpotProbability < 0.0f)
			TargetMachine.JackpotProbability = 0.0f;
		RoundWinChance();
		TargetMachine.SuspicionUpdater();
		UpdateDynamicDisplay();
	}
	
	public void RoundWinChance() {
		TargetMachine.JackpotProbability = (float) Math.Round(TargetMachine.JackpotProbability, 2);
	}
}
