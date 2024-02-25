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

	private double _increaseTimer = -1.0;
    private double _decreaseTimer = -1.0;

    public override void _Ready()
	{
		base._Ready();
		Instance = this;

	}

    public override void _Process(double delta)
    {
        base._Process(delta);

		if (_increaseTimer >= 0.0)
		{
            _increaseTimer -= delta;
			if (_increaseTimer <= 0.0)
			{
                IncreaseWinChance();
				_increaseTimer = 0.1;
            }
        }

        if (_decreaseTimer >= 0.0)
        {
            _decreaseTimer -= delta;
            if (_decreaseTimer <= 0.0)
            {
                DecreaseWinChance();
                _decreaseTimer = 0.1;
            }
        }
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
		_suspicionFactorLabel.Text = "Suspicion Factor: " + TargetMachine.GetSuspicionFactorDisplay();

    }

	public void StartIncreaseWinChance()
	{
		_increaseTimer = 0.0;
	}

	public void StopIncreaseWinChance()
	{
		_increaseTimer = -1.0;
	}

	public void StartDecreaseWinChance()
	{
		_decreaseTimer = 0.0;
	}

	public void StopDecreaseWinChance()
	{
		_decreaseTimer = -1.0;
	}

	private void IncreaseWinChance()
	{
        TargetMachine.JackpotProbability += 0.01f;
        if (TargetMachine.JackpotProbability > 1.0f)
            TargetMachine.JackpotProbability = 1.0f;
        RoundWinChance();
        TargetMachine.SuspicionUpdater();
        UpdateDynamicDisplay();
    }

	private void DecreaseWinChance()
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
