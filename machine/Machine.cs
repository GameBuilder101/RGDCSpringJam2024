using Godot;
using System;

public partial class Machine : Node2D
{
	
	[Export]
	public Texture2D CrapsZero {get; private set;}
	
	
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
	/// The default chance for a guest to win the jackpot.
	/// </summary>
	[Export]
	public float DefaultJackpotProbability { get; private set; }
	
	/// <summary>
	/// The amount of times that a machine gets rolled in a tick.
	/// </summary>
	[Export]
	public int NumRolls { get; private set; }

	/// <summary>
	/// The amount that suspicion increases each tick that a win has not occured. The formula for this is defined in Ready()
	/// </summary>
	[Export]
	public float SuspicionFactor;
	/// <summary>
	/// The default amount that suspicion increases each tick that a win has not occured.
	/// </summary>
	[Export]
	public float DefaultSuspicionFactor { get; private set; }

	/*
	[Export]
	private MachineOptions _optionsMenu;
	*/

	public override void _Ready()
	{
		base._Ready();
		SuspicionUpdater();
		GD.Print(SuspicionFactor);
		Tick();
	}

	public void Tick()
	{
		SuspicionUpdater();
		int Revenue = 0;
		for(int index = 0; index < NumRolls; index++){
			Revenue += PlayCost;
			Revenue -= Roll(0);
		}
		MachineManager.instance.Moola += Revenue;
		MachineManager.instance.Suspicion += (double)SuspicionFactor;
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
	
	public void SuspicionUpdater()
	{
		SuspicionFactor = ((DefaultJackpotProbability/JackpotProbability)*DefaultSuspicionFactor)-DefaultSuspicionFactor;
		if(SuspicionFactor < 0){
			SuspicionFactor = 0;
		}
	}

	/// <summary>
	/// Called when the machine sprite is clicked.
	/// </summary>
	public void Clicked()
	{

	}
}
