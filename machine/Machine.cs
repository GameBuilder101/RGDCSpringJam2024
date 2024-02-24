using Godot;
using System;

public partial class Machine : Node2D
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
	public float JackpotProbability { get; private set; }
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
		Tick();
	}

	public void Tick()
	{
		int Revenue = 0;
		for(int index = 0; index < NumRolls; index++){
			Revenue += PlayCost;
			Revenue -= Roll(0);
		}
		GD.Print(Revenue);
	}

	/// <summary>
	/// Simulates a guest using the machine.
	/// </summary>
	
	public int Roll(ulong seed)
	{
		float random = Probability(seed);
		if(random <= JackpotProbability){
			GD.Print("JACKPOT");
			return JackpotAmount;
		}
		else{
			GD.Print("=(");
			return 0;
		}
	}
	
	public float Probability(ulong seed)
	{
		var RNG = new RandomNumberGenerator();
		RNG.Randomize();
		GD.Seed(seed);
		float value = RNG.Randf();
		return value;
	}

	/// <summary>
	/// Called when the machine sprite is clicked.
	/// </summary>
	public void Clicked()
	{

	}
}
