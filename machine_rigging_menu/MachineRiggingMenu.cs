using Godot;

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

    public void OpenOnMachine(Machine machine)
    {
        TargetMachine = machine;

        //_machineSprite.Texture = machine;
        _playCostLabel.Text = "Payout Per Play: $" + machine.PlayCost;
        _sellCostLabel.Text = "Sell ($" + machine.ShopCost / 2 + ")";
        _jackpotAmountLabel.Text = "Jackpot Cost: $" + machine.JackpotAmount;
        _suspicionFactorLabel.Text = "Suspicion Factor: $" + machine.SuspicionFactor;
        _numRollsLabel.Text = "Rolls Per Guest: " + machine.NumRolls;
        _winChanceLabel.Text = machine.JackpotProbability + "%";
    }

    public void IncreaseWinChance()
    {
        //TargetMachine.JackpotProbability
    }

    public void DecreaseWinChance()
    {
        //TargetMachine.JackpotProbability
    }
}
