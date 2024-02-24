using Godot;
using System;

public partial class Machine : Sprite2D
{
	[Export]
	public String name {get; private set;}
	
	[Export]
	public Texture2D MachineTexture { get; private set; }
	
	
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
	public float JackpotProbability {get; set;}
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
	public float SuspicionFactor;
	/// <summary>
	/// The default amount that suspicion increases each tick that a win has not occured.
	/// </summary>
	[Export]
	public float DefaultSuspicionFactor { get; private set; }

	public Machine() {}

	private Machine(Machine m) {
		this.name = m.name;
		this.MachineTexture = m.MachineTexture;
		this.Texture = m.MachineTexture;
		this.ShopCost = m.ShopCost;
		this.PlayCost = m.PlayCost;
		this.JackpotAmount = m.JackpotAmount;
		this.JackpotProbability = m.DefaultJackpotProbability;
		this.DefaultJackpotProbability = m.DefaultJackpotProbability;
		this.NumRolls = m.NumRolls;
		this.SuspicionFactor = m.DefaultSuspicionFactor;
		this.DefaultSuspicionFactor = m.DefaultSuspicionFactor;
	}

	public Machine copy() {
		return new Machine(this);
	}

	/*
	[Export]
	private MachineOptions _optionsMenu;
	*/

	public override void _Ready()
	{
		base._Ready();
		SuspicionUpdater();
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
	}

	/// <summary>
	/// Simulates a guest using the machine.
	/// </summary>
	
	public int Roll(ulong seed)
	{
		float random = Probability(seed);
		if(random <= JackpotProbability){
			return JackpotAmount;
		}
		else{
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
		SuspicionFactor = DefaultJackpotProbability * DefaultSuspicionFactor / JackpotProbability;
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
