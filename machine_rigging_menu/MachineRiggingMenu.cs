using Godot;

public partial class MachineRiggingMenu : Node2D
{
	public static MachineRiggingMenu Instance { get; private set; }

    private int _targetMachineIndex;
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

    public void HideRiggingMenu()
    {
        base.Hide();
        MachineManager.instance.focus(-1);
    }

    public void Sell()
    {
        MachineManager.instance.Moola += SellCost;
        MachineManager.instance.removeMachine(_targetMachineIndex);
        HideRiggingMenu();
    }

    public void OpenOnMachine(Machine machine)
    {
        TargetMachine = machine;

		_machineNameLabel.Text = machine.name;
        _machineSprite.Texture = machine.MachineTexture;
        _playCostLabel.Text = "Payout Per Play: $" + machine.PlayCost;
        _sellCostLabel.Text = "Sell $" + SellCost;
        _jackpotAmountLabel.Text = "Jackpot Cost: $" + machine.JackpotAmount;
        _suspicionFactorLabel.Text = "Suspicion Factor: " + machine.SuspicionFactor * 100;
        _numRollsLabel.Text = "Rolls Per Guest: " + machine.NumRolls;
        _winChanceLabel.Text = machine.JackpotProbability + "%";
    }

    public void IncreaseWinChance()
    {
        TargetMachine.JackpotProbability += 0.05f;
        if (TargetMachine.JackpotProbability > 1.0f)
            TargetMachine.JackpotProbability = 1.0f;
    }

    public void DecreaseWinChance()
    {
        TargetMachine.JackpotProbability -= 0.05f;
        if (TargetMachine.JackpotProbability < 0.0f)
            TargetMachine.JackpotProbability = 0.0f;
    }
}
