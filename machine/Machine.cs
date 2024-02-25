using Godot;
using System;

public partial class Machine : Sprite2D
{
	[Export]
	public String name {get; private set;}
	
	[Export]
	public Texture2D MachineTexture { get; private set; }
	
	private RandomNumberGenerator moneyRNG;
	private RandomNumberGenerator posRNG;
	
	private int SingleParticleThreshold = 3;
	private int OffsetRadius = 30;
	
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
	public float JackpotProbability { get; set; }
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
	
	public float JackpotSuspicionReduction {get; private set;}

	[Export]
	private Label _belowLabel;

	public Machine() {}

	public void CopyValuesFrom(Machine m) {
		this.name = m.name;
		this.MachineTexture = m.MachineTexture;
		this.Texture = m.MachineTexture;
		this.ShopCost = m.ShopCost;
		this.PlayCost = m.PlayCost;
		this.JackpotAmount = m.JackpotAmount;
		this.JackpotProbability = m.JackpotProbability;
		this.DefaultJackpotProbability = m.DefaultJackpotProbability;
		this.NumRolls = m.NumRolls;
		this.SuspicionFactor = m.SuspicionFactor;
		this.DefaultSuspicionFactor = m.DefaultSuspicionFactor;
		this.moneyRNG = new RandomNumberGenerator();
		this.moneyRNG.Randomize();
		this.posRNG = new RandomNumberGenerator();
		this.posRNG.Randomize();
	}

	/*public Machine copy() {   <--------- Lyx there is something called PACKED SCENES
		return new Machine(this);
	}*/

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
		int rollChange;
		int numJackpots = 0;
		bool HadJackpot = false;
		for(int index = 0; index < NumRolls; index++){
			rollChange = PlayCost - Roll();
			if (rollChange < 0) {
				HadJackpot = true;
				if (NumRolls < SingleParticleThreshold) {
					TextParticleManager.instance.createMoneyChangeParticle(
						"Jackpot!", rollChange, nextParticlePos()
					);
				} else {
					numJackpots += 1;
				}
			} else {
				if (NumRolls < SingleParticleThreshold) {
					TextParticleManager.instance.createMoneyChangeParticle(
						"House win!", rollChange, nextParticlePos()
					);
				}
			}
			Revenue += rollChange;
		}
		if (NumRolls >= SingleParticleThreshold) {
			string text;
			if (numJackpots == 0) {
				text = "";
			} else {
				text = numJackpots + "x jackpots!";
			}
			TextParticleManager.instance.createMoneyChangeParticle(
				text, Revenue, nextParticlePos()
			);
		}
		MachineManager.instance.Moola += Revenue;
		if(HadJackpot == false){
		MachineManager.instance.Suspicion += (double)SuspicionFactor;
		}
	}

	private Vector2 nextParticlePos() {
		return this.Position + Vector2.FromAngle((float) Math.PI + this.posRNG.RandfRange(
			0, (float) (Math.PI / 3)
		)) * OffsetRadius;
	}

	/// <summary>
	/// Simulates a guest using the machine.
	/// </summary>
	
	public int Roll()
	{
		if(moneyRNG.Randf() <= JackpotProbability){
			MachineManager.instance.Suspicion -= JackpotSuspicionReduction;
			return JackpotAmount;
		}
		else{
			return 0;
		}
	}
	
	public void SuspicionUpdater()
	{
		JackpotProbabilityLowerLimit();
		SuspicionFactor = ((DefaultJackpotProbability/JackpotProbability)-1)*DefaultSuspicionFactor;
		if(SuspicionFactor < 0){
			SuspicionFactor = 0;
		}
		_belowLabel.Text = GetSuspicionFactorDisplay();
	}

	public string GetSuspicionFactorDisplay()
	{
		return (Math.Round(SuspicionFactor * 100000.0)) / 1000 + "%";
	}

	public void JackpotProbabilityLowerLimit()
	{
		if(JackpotProbability < 0.001){
			JackpotProbability = (float)0.001;
		}
		JackpotSuspicionReduction = DefaultSuspicionFactor/2;
	}
}
