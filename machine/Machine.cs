using Godot;

public partial class Machine : Node
{
	/// <summary>
	/// Cost of the machine in the shop.
	/// </summary>
	[Export]
	public int ShopCost { get; private set; }

    /// <summary>
    /// Cost for a guest to play the machine (payed to the casino owner).
    /// </summary>
    [Export]
    public int PlayCost { get; private set; }

    /// <summary>
    /// Money lost when a guest wins the jackpot.
    /// </summary>
    [Export]
    public int JackpotAmount { get; private set; }
    /// <summary>
	/// The chance for a guest to win the jackpot. 0 means no chance, and 1 means 100% chance.
	/// </summary>
	[Export]
    public float WinChance { get; private set; }
    /// <summary>
    /// The amount of times that a machine gets rolled in a tick.
    /// </summary>
    [Export]
    public int NumRolls { get; private set; }

    /// <summary>
	/// The amount that suspicion increases each tick that a win has not occured.
	/// </summary>
	[Export]
    public float SuspicionFactor { get; private set; }

    /*
    [Export]
    private MachineOptions _optionsMenu;
    */

    public override void _Ready()
    {
        base._Ready();
    }

    public void Tick()
    {

    }

    /// <summary>
    /// Simulates a guest using the machine.
    /// </summary>
    public void Roll()
    {

    }

    /// <summary>
    /// Called when the machine sprite is clicked.
    /// </summary>
    public void Clicked()
    {

    }
}
